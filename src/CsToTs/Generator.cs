using System;
using System.Collections.Generic;

namespace CsToTs {
    
    public static class Generator {

        public static GenerationContext GetTypeDefinitions(Type type, GenerationOptions options = null) {
            return GetTypeDefinitions(new[] {type}, options);
        }

        public static GenerationContext GetTypeDefinitions(GenerationOptions options, params Type[] types) {
            return GetTypeDefinitions(types, options);
        }

        public static GenerationContext GetTypeDefinitions(params Type[] types) {
            return GetTypeDefinitions(types, null);
        }

        public static GenerationContext GetTypeDefinitions(IEnumerable<Type> types, GenerationOptions options) {
            return Helper.GetTypeDefinitions(types, new GenerationContext(options));
        }
    }
}