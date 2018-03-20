using System.Collections.Generic;
using CsToTs.Definitions;

namespace CsToTs {
    
    public class GenerationContext {

        public GenerationContext(ReflectOptions options) {
            Types = new List<TypeDefinition>();
            Enums = new List<EnumDefinition>();
            Options = options ?? GenerationOptions.Default;
        }
        
        public List<TypeDefinition> Types { get; }
        public List<EnumDefinition> Enums { get; }
        internal ReflectOptions Options { get; } 
    }
}