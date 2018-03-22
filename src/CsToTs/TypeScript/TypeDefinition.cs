using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {
    
    public class TypeDefinition {

        public TypeDefinition(string declaration, IEnumerable<MemberDefinition> members) {
            Declaration = declaration;
            Members = (members ?? Enumerable.Empty<MemberDefinition>()).ToList().AsReadOnly();
        }
        
        public string Declaration { get; }
        public IReadOnlyCollection<MemberDefinition> Members { get; }
    }
}