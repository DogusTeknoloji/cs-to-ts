using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {

    public class MethodDefinition {

        public MethodDefinition(string declaration, IEnumerable<MemberDefinition> parameters = null, 
                                List<string> lines = null, List<string> decorators = null) {
            Declaration = declaration;
            Parameters = (parameters ?? new List<MemberDefinition>()).ToList();
            Lines = lines ?? new List<string>();
            Decorators = decorators ?? new List<string>();
        }

        public string Declaration { get; }
        public List<MemberDefinition> Parameters { get; }
        public List<string> Lines { get; }
        public List<string> Decorators { get; } 
        public string ParameterStr 
            => string.Join(", ", Parameters.Select(m => $"{m.Name}: {m.Type}"));
    }
}