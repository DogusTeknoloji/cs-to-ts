using System.Collections.Generic;

namespace CsToTs.TypeScript {

    public class MethodDefinition {

        public MethodDefinition(string declaration, IDictionary<string, string> parameters, IEnumerable<string> lines) {
            Declaration = declaration;
            Parameters = parameters;
            Lines = lines;
        }

        public string Declaration { get; }
        public IDictionary<string, string> Parameters { get; }
        public IEnumerable<string> Lines { get; }
    }
}