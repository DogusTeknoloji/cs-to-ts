using System.Collections.Generic;

namespace CsToTs.TypeScript {
    
    public class TypeScriptContext {
        
        public TypeScriptContext(TypeScriptOptions options) {
            Types = new List<TypeDefinition>();
            Enums = new List<EnumDefinition>();
            Options = options ?? TypeScriptOptions.Default;
        }
        
        public List<TypeDefinition> Types { get; }
        public List<EnumDefinition> Enums { get; }
        internal TypeScriptOptions Options { get; } 
    }
}