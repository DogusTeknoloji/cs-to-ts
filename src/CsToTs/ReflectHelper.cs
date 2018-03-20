using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using CsToTs.Definitions;

namespace CsToTs {

    public static class ReflectHelper {
        private static bool SkipCheck(string s, ReflectOptions o) =>
            s != null && o.SkipTypePatterns.Any(p => Regex.Match(s, p).Success);

        internal static void PopulateTypeDefinitions(IEnumerable<Type> types, GenerationContext context) {
            foreach (var type in types) {
                if (type.IsEnum) {
                    PopulateEnumDef(type, context);
                }
                else {
                    PopulateTypeDef(type, context);
                }
            }
        }

        private static TypeDefinition PopulateTypeDef(Type type, GenerationContext context) {
            var existing = context.Types.FirstOrDefault(t => t.ClrType == type);
            if (existing != null) return existing;

            if (SkipCheck(type.ToString(), context.Options)) return null;

            TypeDefinition baseType = null;
            if (type.BaseType != typeof(object) && type.BaseType != null) {
                baseType = PopulateTypeDef(type.BaseType, context);
            }

            var memberDefs = GetMemberDefs(type, context);
            var genericArgumentDefs = GetGenericArgumentDefs(type, context);
            var interfaceDefs = GetInterfaceDefs(type, context);
            var actionDefs = GetActionDefs(type, context);
            var def = new TypeDefinition(
                type,
                GetTypeName(type),
                memberDefs,
                genericArgumentDefs,
                baseType,
                interfaceDefs,
                actionDefs
            );

            // constructed generics cannot be declared as types, we must generate their definitions instead
            if (type.IsConstructedGenericType) {
                PopulateTypeDef(type.GetGenericTypeDefinition(), context);
                return def;
            }
            
            context.Types.Add(def);
            return def;
        }

        private static void PopulateEnumDef(Type type, GenerationContext context) {
            var existing = context.Enums.FirstOrDefault(t => t.ClrType == type);
            if (existing != null) return;

            var names = Enum.GetNames(type);
            var members = new List<EnumField>();
            foreach (var name in names) {
                var clrMember = type.GetField(name);
                var value = Convert.ToInt32(Enum.Parse(type, name));

                members.Add(
                    new EnumField(clrMember, name, value)
                );
            }

            var def = new EnumDefinition(type, type.Name, members);
            context.Enums.Add(def);
        }

        private static IEnumerable<MemberDefinition> GetMemberDefs(Type type, GenerationContext context) {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;

            var memberDefs = new List<MemberDefinition>();
            var fields = type.GetFields(bindingFlags).Select(f => new {N = f.Name, T = f.FieldType});
            foreach (var field in fields) {
                memberDefs.Add(
                    new MemberDefinition(field.N, GetMemberType(field.T, context), MemberDeclaration.Field)
                );
            }

            var properties = type.GetProperties(bindingFlags)
                .Select(p => new {N = p.Name, T = p.PropertyType, S = p.CanWrite});
            foreach (var property in properties) {
                memberDefs.Add(
                    new MemberDefinition(
                        property.N,
                        GetMemberType(property.T, context),
                        property.S ? MemberDeclaration.GetSet : MemberDeclaration.Get
                    )
                );
            }

            return memberDefs;
        }

        private static IEnumerable<GenericArgumentDefinition> GetGenericArgumentDefs(Type type,
            GenerationContext context) {
            if (!type.IsGenericType) return Array.Empty<GenericArgumentDefinition>();

            return type.GetGenericArguments()
                .Select(g => {
                        if (!g.IsGenericType) return new GenericArgumentDefinition(g.Name);

                        var constraintTypes = new List<string>();
                        var constraints = g.GetGenericParameterConstraints();
                        for (var i = 0; i < constraints.Length - 1; i++) {
                            var constraint = constraints[i];
                            constraintTypes.Add(constraint.Name);
                            PopulateTypeDef(constraint, context);
                        }

                        return new GenericArgumentDefinition(
                            g.Name,
                            constraintTypes,
                            g.GenericParameterAttributes.HasFlag(
                                GenericParameterAttributes.DefaultConstructorConstraint
                            )
                        );
                    }
                );
        }

        private static IEnumerable<TypeDefinition> GetInterfaceDefs(Type type, GenerationContext context) {
            var interfaces = type.GetInterfaces();
            var interfaceDefs = interfaces.Select(i => PopulateTypeDef(i, context));
            return interfaceDefs.Where(t => t != null);
        }

        private static IEnumerable<ActionDefinition> GetActionDefs(Type type, GenerationContext context) {
            return null; // todo
        }

        private static MemberType GetMemberType(Type type, GenerationContext context) {            
            if (type.IsGenericType) {
                if (typeof(IEnumerable).IsAssignableFrom(type)) {
                    return new MemberType(
                        type, DataType.Object, "Array",
                        new[] {GetMemberType(type.GetGenericArguments()[0], context)}
                    );
                }

                var genericPrms = type.GetGenericArguments().Select(t => GetMemberType(t, context));
                return new MemberType(type, DataType.Object, GetTypeName(type), genericPrms);
            }
            
            if (type.IsGenericParameter) {
                return new MemberType(type, DataType.Object, type.Name);
            }

            if (type.IsEnum) {
                PopulateEnumDef(type, context);
                
                return new MemberType(type, DataType.Object, type.Name);
            }
            
            var typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.Object) {
                if (PopulateTypeDef(type, context) == null) return MemberType.Any;
                
                return new MemberType(type, DataType.Object, type.Name);
            }

            return GetPrimitiveMemberType(typeCode);
        }

        private static MemberType GetPrimitiveMemberType(TypeCode typeCode) {
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
                    return MemberType.Date;
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