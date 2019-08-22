using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {

    public class CtorDefinition {

        public CtorDefinition(IEnumerable<string> lines, string parameters) {
            Lines = (lines?.ToList() ?? new List<string>()).AsReadOnly();
            Parameters = parameters;
        }
        
        public string Parameters { get; }
        public IReadOnlyCollection<string> Lines { get; }
    }
}