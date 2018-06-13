using System;
using System.Collections.Generic;

namespace CsToTs {
    
    public static class Generator {

        public static string GenerateTypeScript(Type type, TypeScriptOptions options = null) 
            => GenerateTypeScript(options, type);

        public static string GenerateTypeScript(params Type[] types) 
            => GenerateTypeScript((IEnumerable<Type>)types);

        public static string GenerateTypeScript(IEnumerable<Type> types) 
            => TypeScript.Helper.GenerateTypeScript(types, TypeScriptOptions.Default);

        public static string GenerateTypeScript(TypeScriptOptions options, params Type[] types) 
            => GenerateTypeScript(options, (IEnumerable<Type>) types);

        public static string GenerateTypeScript(TypeScriptOptions options, IEnumerable<Type> types) 
            => TypeScript.Helper.GenerateTypeScript(types, options ?? TypeScriptOptions.Default);
    }
}