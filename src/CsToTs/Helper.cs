using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text.RegularExpressions;

namespace CsToTs {

    public static class Helper {
        private static bool SkipCheck(string s, GenerationOptions o) => o.SkipTypePatterns.Any(p => Regex.Match(s, p).Success);

        internal static IEnumerable<TypeDefinition> GetTypeDefinitions(IEnumerable<Type> types, GenerationOptions options) {
            var defs = new List<TypeDefinition>();
            foreach (var type in types) {
                PopulateTypeDefs(type, options ?? GenerationOptions.Default, defs);
            }

            return defs.AsReadOnly();
        }

        private static TypeDefinition PopulateTypeDefs(Type type, GenerationOptions options, List<TypeDefinition> defs) {
            var memberDefs = new List<MemberDefinition>();
            TypeDefinition baseType = null;
            if (SkipCheck(type.BaseType.AssemblyQualifiedName, options)) {
                memberDefs.AddRange(GetMemberDefs(type.BaseType, options, defs));
            }
            else {
                baseType = PopulateTypeDefs(type.BaseType, options, defs);
            }
            
            memberDefs.AddRange(GetMemberDefs(type, options, defs));
            
            var def = new TypeDefinition(
                type.Name,
                type.Namespace,
                baseType,
                memberDefs,
                null, // todo
                null // todo
            );
            defs.Add(def);
            
            return def;
        }
        
        private static string GetTypeName(Type type, GenerationOptions options) {
            if (type.IsGenericType) {
                var genericArgs = string.Join(
                    ", ", 
                    type.GetGenericArguments().Select(t => GetTypeName(t, options))
                );
                return $"{type.Name}<{genericArgs}>";
            }
            
            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type)) {
                return $"Array<{GetTypeName(type.GetGenericArguments().First(), options)}>";
            }

            var typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.Object) {
                return SkipCheck(type.BaseType.AssemblyQualifiedName, options)
                    ? "any"
                    : type.Name;
            }

            return GetPrimitiveTypeName(typeCode, options);
        }

        private static string GetPrimitiveTypeName(TypeCode typeCode, GenerationOptions options) {
            switch (typeCode) {
                case TypeCode.Boolean:
                    return "boolean";
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
                    return "number";
                case TypeCode.Char:
                case TypeCode.String:
                    return "string";
                case TypeCode.DateTime:
                    return options.DateForDateTime ? "Date" : "string";
                default:
                    return "any";
            }
        }

        private static IEnumerable<MemberDefinition> GetMemberDefs(Type type, GenerationOptions options, List<TypeDefinition> defs) {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            var members = type.GetFields(bindingFlags).Select(f => new { N = f.Name, T = f.FieldType })
                .Concat(type.GetProperties(bindingFlags).Select(p => new { N = p.Name, T = p.PropertyType }));

            foreach (var member in members) {
                var typeName = GetTypeName(member.T, options);
            }

            return null;
        }

        /// <summary>
        /// Returns type definition with generic arguments and their constraints
        /// </summary>
        private static string GetTypeDeclaration(Type type, GenerationOptions options) {
            var typeName = GetTypeName(type, options);
            if (!type.IsGenericType) return typeName;

            var constraints = string.Join(
                ", ",
                type.GetGenericArguments().Select(g => {
                    var newConstraint =
                        g.GenericParameterAttributes.HasFlag(GenericParameterAttributes.DefaultConstructorConstraint)
                            ? $"{{new(): {g.Name}}}"
                            : "";
                    var typeConstraints = string.Join(
                        " & ",
                        g.GetGenericParameterConstraints()
                            .Select(c => c.Name)
                    );

                    return string.IsNullOrEmpty(newConstraint)
                        ? typeConstraints
                        : $"{newConstraint} & {typeConstraints}";
                })
            );

            return $"{typeName}<{constraints}>";
        }
    }
}