using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace CsToTs.Definitions {

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
            var genericPrmDefs = GetGenericPrmDefs(type, context);
            var interfaceDefs = GetInterfaceDefs(type, context);
            var def = new TypeDefinition(
                type,
                Helper.GetTypeName(type),
                memberDefs,
                genericPrmDefs,
                baseType,
                interfaceDefs
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

            var memberDefs = type.GetFields(bindingFlags)
                .Select(f => GetMemberDef(f, f.FieldType, context))
                .ToList();

            memberDefs.AddRange(
                type.GetProperties(bindingFlags).Select(p => GetMemberDef(p, p.PropertyType, context))
            );
            
            return memberDefs;
        }

        private static MemberDefinition GetMemberDef(MemberInfo member, Type type, GenerationContext context) {
            return new MemberDefinition(member, Helper.GetTypeName(type), GetMemberType(type, context));
        }

        private static MemberType GetMemberType(Type type, GenerationContext context) {
            var typeCode = Type.GetTypeCode(type);
            bool isEnumerable;
            TypeDefinition typeDef = null;
            IEnumerable<MemberType> genericArgs = null;
            
            if (typeCode == TypeCode.Object) {
                if (type.IsGenericType) {
                    isEnumerable = typeof(IEnumerable).IsAssignableFrom(type);
                    genericArgs = type.GetGenericArguments().Select(g => GetMemberType(g, context));
                }

                typeDef = PopulateTypeDef(type, context);
            }

            return new MemberType(type, typeCode, isEnumerable, typeDef, genericArgs);
        }

        private static IEnumerable<GenericParameterDefinition> GetGenericPrmDefs(Type type,
            GenerationContext context) {
            if (!type.IsGenericType) return Array.Empty<GenericParameterDefinition>();

            return type.GetGenericArguments()
                .Select(g => {
                    if (!g.IsGenericType) return new GenericParameterDefinition(g.Name);

                    var constraintTypes = new List<string>();
                    var constraints = g.GetGenericParameterConstraints();
                    for (var i = 0; i < constraints.Length - 1; i++) {
                        var constraint = constraints[i];
                        constraintTypes.Add(constraint.Name);
                        PopulateTypeDef(constraint, context);
                    }

                    return new GenericParameterDefinition(
                        g.Name,
                        constraintTypes,
                        g.GenericParameterAttributes.HasFlag(
                            GenericParameterAttributes.DefaultConstructorConstraint
                        )
                    );
                });
        }

        private static IEnumerable<TypeDefinition> GetInterfaceDefs(Type type, GenerationContext context) {
            var interfaces = type.GetInterfaces();
            var interfaceDefs = interfaces.Select(i => PopulateTypeDef(i, context));
            return interfaceDefs.Where(t => t != null);
        }
    }
}