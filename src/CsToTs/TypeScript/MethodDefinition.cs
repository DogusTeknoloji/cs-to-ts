using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {

    public class MethodDefinition {

        public MethodDefinition(string declaration, IEnumerable<MemberDefinition> parameters = null, 
                                IEnumerable<string> lines = null, IEnumerable<string> decorators = null) {
            Declaration = declaration;
            Parameters = (parameters as IList<MemberDefinition>) ?? new List<MemberDefinition>();
            Lines = (lines as IList<string>) ?? new List<string>();
            Decorators = (decorators as IList<string>) ?? new List<string>();
        }

        public string Declaration { get; }
        public IList<MemberDefinition> Parameters { get; }
        public IList<string> Lines { get; }
        public IList<string> Decorators { get; } 
        public string ParameterStr 
            => string.Join(", ", Parameters.Select(m => $"{m.Name}: {m.Type}"));
    }
}