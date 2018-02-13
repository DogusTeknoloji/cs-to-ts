using System;

namespace CsToTs {
    
    public class MemberDefinition {

        public MemberDefinition(string name, string typeName) {
            Name = name;
            TypeName = typeName;
        }
        
        public string Name { get; }
        public string TypeName { get; }
    }
}