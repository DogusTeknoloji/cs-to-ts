using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CsToTs {

    public static class Helper {
        private static bool SkipCheck(string s, GenerationOptions o) =>
            o.SkipTypePatterns.Any(p => Regex.Match(s, p).Success);

        internal static IEnumerable<TypeDefinition> GetTypeDefinitions(IEnumerable<Type> types,
            GenerationOptions options) {
            var defs = new List<TypeDefinition>();
            foreach (var type in types) {
                PopulateTypeDef(type, options ?? GenerationOptions.Default, defs);
            }

            return defs.AsReadOnly();
        }

        private static TypeDefinition PopulateTypeDef(Type type, GenerationOptions options, List<TypeDefinition> defs) {
            if (SkipCheck(type.AssemblyQualifiedName, options)) return null;
            
            TypeDefinition baseType = null;
            if (type.BaseType != null) {
                baseType = PopulateTypeDef(type.BaseType, options, defs);
            }

            var memberDefs = GetMemberDefs(type, options, defs);
            var genericArgumentDefs = GetGenericArgumentDefs(type, options, defs);
            var interfaceDefs = GetInterfaceDefs(type, options, defs);
            var actionDefs = GetActionDefs(type, options, defs);
            var def = new TypeDefinition(
                type,
                GetTypeName(type),
                type.Namespace,
                memberDefs,
                genericArgumentDefs,
                baseType,
                interfaceDefs,
                actionDefs,
                type.IsAbstract,
                type.IsInterface
            );

            // constructed generics cannot be declared as types, we must generate their definitions instead
            if (type.IsConstructedGenericType) {
                PopulateTypeDef(type.GetGenericTypeDefinition(), options, defs);
                return def;
            }
            
            defs.Add(def);
            return def;
        }

        private static IEnumerable<MemberDefinition> GetMemberDefs(Type type, GenerationOptions options,
            List<TypeDefinition> defs) {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            var memberDefs = new List<MemberDefinition>();
            var fields = type.GetFields(bindingFlags).Select(f => new {N = f.Name, T = f.FieldType});
            foreach (var field in fields) {
                memberDefs.Add(
                    new MemberDefinition(field.N, GetMemberType(field.T, options, defs), MemberDeclaration.Field)
                );
            }

            var properties = type.GetProperties(bindingFlags)
                .Select(p => new {N = p.Name, T = p.PropertyType, S = p.CanWrite});
            foreach (var property in properties) {
                memberDefs.Add(
                    new MemberDefinition(
                        property.N,
                        GetMemberType(property.T, options, defs),
                        property.S ? MemberDeclaration.GetSet : MemberDeclaration.Get
                    )
                );
            }

            return memberDefs;
        }

        private static IEnumerable<GenericArgumentDefinition> GetGenericArgumentDefs(Type type,
            GenerationOptions options, List<TypeDefinition> defs) {
            if (!type.IsGenericType) return Array.Empty<GenericArgumentDefinition>();

            return type.GetGenericArguments()
                .Select(g => {
                        return new GenericArgumentDefinition(
                            g.Name,
                            g.IsGenericType ? g.GetGenericParameterConstraints().Select(c => c.Name) : null,
                            g.IsGenericType && g.GenericParameterAttributes.HasFlag(
                                GenericParameterAttributes.DefaultConstructorConstraint)
                        );
                    }
                );
        }

        private static IEnumerable<TypeDefinition> GetInterfaceDefs(Type type, GenerationOptions options,
            List<TypeDefinition> defs) {
            var interfaces = type.GetInterfaces();
            return interfaces.Select(i => PopulateTypeDef(type, options, defs)).Where(t => t != null);
        }

        private static IEnumerable<ActionDefinition> GetActionDefs(Type type, GenerationOptions options,
            List<TypeDefinition> defs) {
            return null; // todo
        }

        private static MemberType GetMemberType(Type type, GenerationOptions options, List<TypeDefinition> defs) {            
            if (type.IsGenericType) {
                if (typeof(IEnumerable).IsAssignableFrom(type)) {
                    return new MemberType(
                        type, DataType.Object, "Array",
                        new[] {GetMemberType(type.GetGenericArguments()[0], options, defs)}
                    );
                }

                var genericPrms = type.GetGenericArguments().Select(t => GetMemberType(t, options, defs));
                return new MemberType(type, DataType.Object, GetTypeName(type), genericPrms);
            }
            
            if (type.IsGenericParameter) {
                return new MemberType(type, DataType.Object, type.Name);
            }

            var typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.Object) {
                if (SkipCheck(type.AssemblyQualifiedName, options)) return MemberType.Any;

                if (defs.All(d => d.ClrType != type)) {
                    PopulateTypeDef(type, options, defs);
                }
                
                return new MemberType(type, DataType.Object, type.Name);
            }

            return GetPrimitiveTypeName(typeCode, options);
        }

        private static MemberType GetPrimitiveTypeName(TypeCode typeCode, GenerationOptions options) {
            switch (typeCode) {
                case TypeCode.Boolean:
                    return MemberType.Boolean;
                case TypeCode.Byte:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.SByte:
                case TypeCode.Single:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return MemberType.Number;
                case TypeCode.Char:
                case TypeCode.String:
                    return MemberType.String;
                case TypeCode.DateTime:
                    return options.DateForDateTime ? MemberType.Date : MemberType.String;
                default:
                    return MemberType.Any;
            }
        }

        private static string GetTypeName(Type type) {
            var idx = type.Name.IndexOf('`');
            return idx == -1 ? type.Name : type.Name.Substring(0, idx);
        }

        internal static string GetFullName(TypeDefinition def) {
            if (!def.GenericArguments.Any()) return def.Name;

            var genericArgs = string.Join(
                ", ",
                def.GenericArguments.Select(g => {
                    var constraints = new List<string>();
                    if (g.HasNewConstraint) {
                        constraints.Add($"new(): {{{g.Name}}}");
                    }

                    constraints.AddRange(g.TypeConstraints);
                    return string.Join(", ", constraints);
                })
            );
            return $"{def.Name}<{genericArgs}>";
        }
    }
}