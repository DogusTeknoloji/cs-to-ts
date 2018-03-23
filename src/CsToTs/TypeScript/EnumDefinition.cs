using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {
    
    public class EnumDefinition {

        public EnumDefinition(string name, IEnumerable<EnumField> fields) {
            Name = name;
            Fields = (fields ?? Array.Empty<EnumField>()).ToList().AsReadOnly();
        }
        
        public Type ClrType { get; }
        public string Name { get; }
        public IReadOnlyCollection<EnumField> Fields { get; }
    }
}