using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Text;
using HandlebarsDotNet;

namespace CsToTs.TypeScript {

    public static class TypeScriptHelper {
        private static readonly Lazy<string> _lazyTemplate = new Lazy<string>(GetDefaultTemplate);
        private static string Template => _lazyTemplate.Value;

        public static string GenerateTypeScript(Definitions.GenerationContext context, TypeScriptOptions options = null) {
            if (options == null) {
                options = TypeScriptOptions.Default;
            }
            
            var tsContext = ConvertToTypeScript(context, options);

            var template = options != null && !string.IsNullOrEmpty(options.Template)
                ? options.Template
                : Template;

            Handlebars.Configuration.TextEncoder = SimpleEncoder.Instance;

            var generator = Handlebars.Compile(template);
            return generator(tsContext);
        }

        private static TypeScriptContext ConvertToTypeScript(
                Definitions.GenerationContext context, 
                TypeScriptOptions options) {
            var tsContext = new TypeScriptContext(options);

            foreach (var @enum in context.Enums) {
                var tsEnum = new EnumDefinition(
                    @enum.Name,
                    @enum.Fields.Select(f => new EnumField(f.Name, f.Value.ToString()))
                );
                tsContext.Enums.Add(tsEnum);
            }

            foreach (var type in context.Types) {
                var tsType = new TypeDefinition(
                    GetDeclaration(type, context, tsContext),
                    type.Members.Select(m => 
                        new MemberDefinition(m.Name, GetTypeRefName(m.Type, context, options))
                    )
                );
                tsContext.Types.Add(tsType);
            }

            return tsContext;
        }

        private static string GetDefaultTemplate() {
            var ass = typeof(Generator).Assembly;
            var resourceName = ass.GetManifestResourceNames().First(r => r.Contains("template.handlebars"));
            using (var reader = new StreamReader(ass.GetManifestResourceStream(resourceName), Encoding.UTF8)) {
                return reader.ReadToEnd();
            }
        }

        private static string GetDeclaration(
                Definitions.TypeDefinition type, 
                Definitions.GenerationContext context, 
                TypeScriptContext tsContext) {
            var options = tsContext.Options;
            string declaration;
            var baseTypeStr = string.Empty;
            var implements = type.ImplementedInterfaces.ToList();
            if (type.TypeKind == Definitions.TypeKind.Interface || options.UseInterfaceForClasses) {
                declaration = $"export interface ${GetFullTypeName(type)}";
                if (type.BaseType != null) {
                    implements.Insert(0, type.BaseType);
                }
            }
            else {
                declaration = $"export class ${GetFullTypeName(type)}";
                if (type.BaseType != null) {
                    baseTypeStr = GetTypeRefName(type.BaseType.ClrType, context, options);
                }
            }

            var implementStr = string.Join(
                ", ",
                implements.Select(i => GetTypeRefName(i.ClrType, context, options))
            );

            if (!string.IsNullOrEmpty(baseTypeStr)) {
                declaration += $" extends {baseTypeStr}";
            }

            if (!string.IsNullOrEmpty(implementStr)) {
                declaration += $" implements {implementStr}";
            }

            return declaration;
        }

        private static string GetFullTypeName(Definitions.TypeDefinition type) {
            if (!type.GenericParameters.Any()) return type.Name;

            var genericArgs = string.Join(
                ", ",
                type.GenericParameters.Select(g => {
                    var genericArg = g.Name;
                    var constraints = g.TypeConstraints.ToList();
                    if (g.HasNewConstraint) {
                        constraints.Add("new() => " + g.Name);
                    }

                    if (constraints.Any()) {
                        genericArg += $" extends {string.Join(", ", constraints)}";
                    }

                    return genericArg;
                })
            );
            return $"{type.Name}<{genericArgs}>";
        }

        private static string GetTypeRefName(
                Definitions.MemberType memberType, 
                Definitions.GenerationContext context, 
                TypeScriptOptions options) {
            if (memberType.IsEnumerable) {
                return $"Array<{GetTypeRefName(memberType.GenericArguments.First(), context, options)}>";
            }
            
            if (memberType.GenericArguments.Any()) {
                var genericPrms = memberType.GenericArguments.Select(t => GetTypeRefName(t, context, options));
                return $"{memberType.}<{string.Join(", ", genericPrms)}>";
            }

            var typeCode = Type.GetTypeCode(type);
            if (Type.GetTypeCode(type) == TypeCode.Object) {
                if (context.Types.All(t => t.ClrType != type)) return "any";

                return Helper.GetTypeName(type);
            }

            if (type.IsEnum) {
                return context.Enums.Any(e => e.ClrType == type) ? type.Name : "any";
            }

            return GetPrimitiveMemberType(typeCode, options);
        }

        private static string GetPrimitiveMemberType(TypeCode typeCode, TypeScriptOptions options) {
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
    }
}
