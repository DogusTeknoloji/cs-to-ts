using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {

    public class CtorDefinition {

        public CtorDefinition(IEnumerable<string> lines, string parameters) {
            Lines = lines == null ? new List<string>() : lines.ToList();
            Parameters = parameters;
        }
        
        public string Parameters { get; }
        public List<string> Lines { get; }
    }
}