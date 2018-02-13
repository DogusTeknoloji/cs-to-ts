using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs {
    
    public class InterfaceDefinition {

        public InterfaceDefinition(
            string name,
            string @namespace,
            IEnumerable<MemberDefinition> members,
            IEnumerable<GenericParameterDefinition> genericParameters = null, 
            IEnumerable<InterfaceDefinition> implementedInterfaces = null) {

            Name = name;
            Namespace = @namespace;
            NamespacePath = string.IsNullOrEmpty(Namespace)
                ? Array.Empty<string>()
                : Namespace.Split('.');

            Members = (members ?? Array.Empty<MemberDefinition>()).ToList().AsReadOnly();
            GenericParameters = (genericParameters ?? Array.Empty<GenericParameterDefinition>()).ToList().AsReadOnly();
            ImplementedInterfaces = (implementedInterfaces ?? Array.Empty<InterfaceDefinition>()).ToList().AsReadOnly();
        }
        
        public string Name { get; }
        public string Namespace { get; }
        public IReadOnlyCollection<string> NamespacePath { get; }
        public IReadOnlyCollection<MemberDefinition> Members { get; }
        public IReadOnlyCollection<GenericParameterDefinition> GenericParameters { get; }
        public IReadOnlyCollection<InterfaceDefinition> ImplementedInterfaces { get; }

        public virtual string FormatName() {
            return $"{Name}";
        }
    }
}