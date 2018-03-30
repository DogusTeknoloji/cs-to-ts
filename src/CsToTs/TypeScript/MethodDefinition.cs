using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {

    public class MethodDefinition {

        public MethodDefinition(string declaration, IEnumerable<MemberDefinition> parameters = null, 
                                List<string> lines = null) {
            Declaration = declaration;
            Parameters = parameters == null ? new List<MemberDefinition>() : parameters.ToList();
            Lines = lines == null ? new List<string>() : lines.ToList();
        }

        public string Declaration { get; }
        public List<MemberDefinition> Parameters { get; }
        public List<string> Lines { get; }

        public string ParameterStr 
            => string.Join(", ", Parameters.Select(m => $"{m.Name}: {m.Type}"));
    }
}