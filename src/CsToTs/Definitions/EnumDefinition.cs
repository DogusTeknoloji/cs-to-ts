using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs.Definitions {
    
    public class EnumDefinition {

        public EnumDefinition(Type clrType, string name, IEnumerable<EnumField> fields) {
            ClrType = clrType;
            Name = name;
            Fields = (fields ?? Array.Empty<EnumField>()).ToList().AsReadOnly();
        }
        
        public Type ClrType { get; }
        public string Name { get; }
        public string Namespace { get; }
        public IReadOnlyCollection<EnumField> Fields { get; }
    }
}