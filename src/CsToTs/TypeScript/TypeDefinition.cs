using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs.TypeScript {
    
    public class TypeDefinition {

        public TypeDefinition(Type clrType, string declaration) {
            ClrType = clrType;
            Declaration = declaration;
            Members = new List<MemberDefinition>();
        }
        
        public Type ClrType { get; }
        public string Declaration { get; }
        public List<MemberDefinition> Members { get; }
    }
}