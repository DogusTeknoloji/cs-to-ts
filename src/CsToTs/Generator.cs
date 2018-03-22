using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CsToTs.Definitions;
using CsToTs.TypeScript;
using HandlebarsDotNet;

namespace CsToTs {
    
    public static class Generator {

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

        public static string GenerateTypeScript(Type type, TypeScriptOptions options = null) {
            return GenerateTypeScript(options, type);
        }

        public static string GenerateTypeScript(TypeScriptOptions options = null, params Type[] types) {
            return GenerateTypeScript(types, options);
        }

        public static string GenerateTypeScript(IEnumerable<Type> types, TypeScriptOptions options = null) {
            var context = new GenerationContext(options);
            ReflectHelper.PopulateTypeDefinitions(types, context);
            return TypeScriptHelper.GenerateTypeScript(context, options);
        }
    }
}