using System;
using System.Collections.Generic;

namespace CsToTs {
    
    public static class Generator {

        public static IEnumerable<TypeDefinition> GetTypeDefinitions(Type type, GenerationOptions options) {
            return GetTypeDefinitions(new[] {type}, options);
        }

        public static IEnumerable<TypeDefinition> GetTypeDefinitions(GenerationOptions options, params Type[] types) {
            return GetTypeDefinitions(types, options);
        }


        public static IEnumerable<TypeDefinition>GetTypeDefinitions(IEnumerable<Type> types, 
                                                                    GenerationOptions options) {
            return Helper.GetTypeDefinitions(types, options);
        }
        
        public static string GenerateTypeScript(Type type, GenerationOptions options) {
            return GenerateTypeScript(new[] {type}, options);
        }

        public static string GenerateTypeScript(GenerationOptions options, params Type[] types) {
            return GenerateTypeScript(types, options);
        }


        public static string GenerateTypeScript(IEnumerable<Type> types, GenerationOptions options) {
            var definitions = GetTypeDefinitions(types, options);
            return string.Empty; //todo
        }
    }
}