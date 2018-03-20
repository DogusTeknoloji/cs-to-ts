using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using HandlebarsDotNet;

namespace CsToTs {
    
    public static class Generator {
        private static readonly Lazy<string> _lazyTemplate = new Lazy<string>(GetDefaultTemplate);
        internal static string Template => _lazyTemplate.Value;

        public static GenerationContext GetTypeDefinitions(Type type, ReflectOptions options = null) {
            return GetTypeDefinitions(options, type);
        }

        public static GenerationContext GetTypeDefinitions(ReflectOptions options = null, params Type[] types) {
            return GetTypeDefinitions(types, options);
        }

        public static GenerationContext GetTypeDefinitions(IEnumerable<Type> types, ReflectOptions options = null) {
            var context = new GenerationContext(options);
            ReflectHelper.PopulateTypeDefinitions(types, context);
            return context;
        }

        public static string GenerateTypeScript(Type type, GenerationOptions options = null) {
            return GenerateTypeScript(options, type);
        }

        public static string GenerateTypeScript(GenerationOptions options = null, params Type[] types) {
            return GenerateTypeScript(types, options);
        }

        public static string GenerateTypeScript(IEnumerable<Type> types, GenerationOptions options = null) {
            var context = new GenerationContext(options);
            ReflectHelper.PopulateTypeDefinitions(types, context);
            var template = options != null && string.IsNullOrEmpty(options.Template) 
                ? options.Template 
                : Template;
            
            Handlebars.Configuration.TextEncoder = SimpleEncoder.Instance;

            var generator = Handlebars.Compile(template);
            return generator(context);
        }
        
        private static string GetDefaultTemplate() {
            var ass = typeof(Generator).Assembly;
            var resourceName = ass.GetManifestResourceNames().First(r => r.Contains("template.handlebars"));
            using (var reader = new StreamReader(ass.GetManifestResourceStream(resourceName), Encoding.UTF8)) {
                return reader.ReadToEnd();
            }
        }
    }
}