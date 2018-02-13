using System.Collections.Generic;

namespace CsToTs {
    
    public class TypeDefinition: InterfaceDefinition {

        public TypeDefinition(
            string name,
            string @namespace,
            TypeDefinition baseType,
            IEnumerable<MemberDefinition> members,
            IEnumerable<GenericParameterDefinition> genericParameters = null, 
            IEnumerable<InterfaceDefinition> implementedInterfaces = null)
            : base(name, @namespace, members, genericParameters, implementedInterfaces) {
        }
        
        public bool IsAbstract { get; }
        public TypeDefinition BaseType { get; }
    }
}
