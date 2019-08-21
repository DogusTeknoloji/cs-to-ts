using System.Collections.Generic;

namespace CsToTs.TypeScript {
    
    public class MemberDefinition {

        public MemberDefinition(string name, string type, List<string> decorators = null) {
            Name = name;
            Type = type;
            Decorators = decorators ?? new List<string>();
        }
        
        public string Name { get; }
        public string Type { get; }
        public List<string> Decorators { get; set; }
    }
}