using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs {
    
    public class TypeDefinition {

        public TypeDefinition(
            Type clrType,
            string name,
            IEnumerable<MemberDefinition> members,
            IEnumerable<GenericArgumentDefinition> genericArguments = null,
            TypeDefinition baseType = null,
            IEnumerable<TypeDefinition> implementedInterfaces = null,
            IEnumerable<ActionDefinition> actions = null) {

            ClrType = clrType;
            Name = name;
            Namespace = clrType.Namespace;
            Members = (members ?? Array.Empty<MemberDefinition>()).ToList().AsReadOnly();
            GenericArguments = (genericArguments ?? Array.Empty<GenericArgumentDefinition>()).ToList().AsReadOnly();
            ImplementedInterfaces = (implementedInterfaces ?? Array.Empty<TypeDefinition>()).ToList().AsReadOnly();
            Actions = (actions ?? Array.Empty<ActionDefinition>()).ToList().AsReadOnly();
            BaseType = baseType;
            IsAbstract = clrType.IsAbstract;
            TypeKind = clrType.IsEnum ? TypeKind.Enum
                : clrType.IsInterface ? TypeKind.Interface 
                                      : TypeKind.Type;
            
            FullName = Helper.GetFullName(this);
            
            NamespacePath = string.IsNullOrEmpty(Namespace)
                ? Array.Empty<string>()
                : Namespace.Split('.');
        }
        
        public Type ClrType { get; }
        public string Name { get; }
        public string Namespace { get; }
        public IReadOnlyCollection<MemberDefinition> Members { get; }
        public IReadOnlyCollection<GenericArgumentDefinition> GenericArguments { get; }
        public IReadOnlyCollection<TypeDefinition> ImplementedInterfaces { get; }
        public IReadOnlyCollection<ActionDefinition> Actions { get; }
        public TypeDefinition BaseType { get; }
        public bool IsAbstract { get; }
        public TypeKind TypeKind { get; }
        
        public string FullName { get; }
        public IReadOnlyCollection<string> NamespacePath { get; }
    }
}
