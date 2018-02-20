using System;
using System.Collections.Generic;

namespace CsToTs {
    
    public static class Generator {

        public static IEnumerable<TypeDefinition> GetTypeDefinitions(Type type, GenerationOptions options = null) {
            return GetTypeDefinitions(new[] {type}, options);
        }

        public static IEnumerable<TypeDefinition> GetTypeDefinitions(GenerationOptions options, params Type[] types) {
            return GetTypeDefinitions(types, options);
        }

        public static IEnumerable<TypeDefinition> GetTypeDefinitions(params Type[] types) {
            return GetTypeDefinitions(types, null);
        }

        public static IEnumerable<TypeDefinition>GetTypeDefinitions(IEnumerable<Type> types, GenerationOptions options) {
            return Helper.GetTypeDefinitions(types, options);
        }
    }
}