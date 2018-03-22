using System.Collections.Generic;
using CsToTs.Definitions;

namespace CsToTs.Definitions {
    
    public class GenerationContext {

        public GenerationContext(ReflectOptions options) {
            Types = new List<TypeDefinition>();
            Enums = new List<EnumDefinition>();
            Options = options ?? ReflectOptions.Default;
        }
        
        public List<TypeDefinition> Types { get; }
        public List<EnumDefinition> Enums { get; }
        internal ReflectOptions Options { get; } 
    }
}