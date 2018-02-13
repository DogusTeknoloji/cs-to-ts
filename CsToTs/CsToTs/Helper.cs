using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CsToTs {

    public static class Helper {
        private static readonly string[] SkipTypePatterns = {
            @"System\.*"
        };

        private static bool SkipCheck(string s) => SkipTypePatterns.Any(p => Regex.Match(s, p).Success);

        public static IEnumerable<TypeDefinition> GetTypeDefinitions(IEnumerable<Type> types, GenerationOptions options) {
            var defs = new List<TypeDefinition>();
            foreach (var type in types) {
                PopulateTypeDefs(type, defs, options);
            }

            return defs.AsReadOnly();
        }

        private static TypeDefinition PopulateTypeDefs(Type type, List<TypeDefinition> defs, GenerationOptions options) {
            var memberDefs = new List<MemberDefinition>();
            TypeDefinition baseType = null;
            if (SkipCheck(type.BaseType.AssemblyQualifiedName)) {
                memberDefs.AddRange(GetMemberDefs(type.BaseType, defs, options));
            }
            else {
                baseType = PopulateTypeDefs(type.BaseType, defs, options);
            }
            
            memberDefs.AddRange(GetMemberDefs(type, defs, options));
            
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
                var genericArgs = string.Join(", ", type.GetGenericArguments().Select(t => GetTypeName(t, options)));
                return $"{type.Name}<{genericArgs}>";
            }
            
            if (type != typeof(string) && typeof(IEnumerable).IsAssignableFrom(type)) {
                return $"Array<{GetTypeName(type.GetGenericArguments().First(), options)}>";
            }

            var typeCode = Type.GetTypeCode(type);
            if (typeCode == TypeCode.Object) {
                return SkipCheck(type.BaseType.AssemblyQualifiedName)
                    ? "any"
                    : type.Name;
            }

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
                    return options.UseDateForDateTime ? "Date" : "string";
                default:
                    return "any";
            }
        }

        private static IEnumerable<MemberDefinition> GetMemberDefs(Type type, List<TypeDefinition> defs, GenerationOptions options) {
            const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            var members = type.GetFields(bindingFlags).Select(f => new { N = f.Name, T = f.FieldType })
                .Concat(type.GetProperties(bindingFlags).Select(p => new { N = p.Name, T = p.PropertyType }));

            foreach (var member in members) {
                var typeName = GetTypeName(member.T, options);
            }

            return null;
        }
    }
}