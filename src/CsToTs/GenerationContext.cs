using System.Collections.Generic;

namespace CsToTs {
    
    public class GenerationContext {

        public GenerationContext(GenerationOptions options) {
            Options = options ?? GenerationOptions.Default;
            Types = new List<TypeDefinition>();
            Enums = new List<EnumDefinition>();
        }
        
        public GenerationOptions Options { get; }
        public List<TypeDefinition> Types { get; }
        public List<EnumDefinition> Enums { get; }
    }
}