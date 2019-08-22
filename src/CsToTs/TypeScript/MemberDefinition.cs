using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {
    
    public class MemberDefinition {

        public MemberDefinition(string name, string type, IEnumerable<string> decorators = null) {
            Name = name;
            Type = type;
            Decorators = (decorators?.ToList() ?? new List<string>()).AsReadOnly();
        }
        
        public string Name { get; }
        public string Type { get; }
        public IReadOnlyCollection<string> Decorators { get; }
    }
}