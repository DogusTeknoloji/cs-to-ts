using System;

namespace CsToTs {
    
    public static class Generator {

        public static string GenerateTypeScript(Type type, TypeScriptOptions options = null) {
            return GenerateTypeScript(options, type);
        }

        public static string GenerateTypeScript(TypeScriptOptions options = null, params Type[] types) {
            return TypeScript.Helper.GenerateTypeScript(types, options);
        }
    }
}