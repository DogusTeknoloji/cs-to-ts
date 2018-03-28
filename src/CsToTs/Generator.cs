using System;

namespace CsToTs {
    
    public static class Generator {

        public static string GenerateTypeScript(Type type, TypeScriptOptions options = null) 
            => GenerateTypeScript(options, type);

        public static string GenerateTypeScript(params Type[] types) 
            => TypeScript.Helper.GenerateTypeScript(types, TypeScriptOptions.Default);

        public static string GenerateTypeScript(TypeScriptOptions options, params Type[] types) 
            => TypeScript.Helper.GenerateTypeScript(types, options ?? TypeScriptOptions.Default);
    }
}