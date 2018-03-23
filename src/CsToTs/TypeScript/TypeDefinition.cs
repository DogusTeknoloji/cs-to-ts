using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {
    
    public class TypeDefinition {

        public TypeDefinition(Type clrType) {
            ClrType = clrType;
            Members = new List<MemberDefinition>();
        }
        
        public Type ClrType { get; }
        public string Declaration { get; set; }
        public List<MemberDefinition> Members { get; }
    }
}