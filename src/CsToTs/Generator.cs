using System;

namespace CsToTs {
    
    public static class Generator {

        public static string GenerateTypeScript(Type type, TypeScriptOptions options = null) => GenerateTypeScript(options, type);

        public static string GenerateTypeScript(TypeScriptOptions options = null, params Type[] types) => TypeScript.Helper.GenerateTypeScript(types, options ?? TypeScriptOptions.Default);
    }
}