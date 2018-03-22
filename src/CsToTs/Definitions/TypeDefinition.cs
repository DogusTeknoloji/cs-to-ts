using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs.Definitions {
    
    public class TypeDefinition {

        public TypeDefinition(
            Type clrType,
            string name,
            IEnumerable<MemberDefinition> members,
            IEnumerable<GenericParameterDefinition> genericParameters = null,
            TypeDefinition baseType = null,
            IEnumerable<TypeDefinition> implementedInterfaces = null) {

            ClrType = clrType;
            Name = name;
            Namespace = clrType.Namespace;
            Members = (members ?? Array.Empty<MemberDefinition>()).ToList().AsReadOnly();
            GenericParameters = (genericParameters ?? Array.Empty<GenericParameterDefinition>()).ToList().AsReadOnly();
            ImplementedInterfaces = (implementedInterfaces ?? Array.Empty<TypeDefinition>()).ToList().AsReadOnly();
            BaseType = baseType;
            IsAbstract = clrType.IsAbstract;
            TypeKind = clrType.IsInterface ? TypeKind.Interface : TypeKind.Type;
        }
        
        public Type ClrType { get; }
        public string Name { get; }
        public string Namespace { get; }
        public IReadOnlyCollection<MemberDefinition> Members { get; }
        public IReadOnlyCollection<GenericParameterDefinition> GenericParameters { get; }
        public IReadOnlyCollection<TypeDefinition> ImplementedInterfaces { get; }
        public TypeDefinition BaseType { get; }
        public bool IsAbstract { get; }
        public TypeKind TypeKind { get; }
    }
}
