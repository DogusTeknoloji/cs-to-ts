using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs {
    
    public class TypeDefinition {

        public TypeDefinition(
            Type clrType,
            string name,
            string @namespace,
            IEnumerable<MemberDefinition> members,
            IEnumerable<GenericArgumentDefinition> genericArguments = null,
            IEnumerable<TypeDefinition> implementedInterfaces = null,
            IEnumerable<ActionDefinition> actions = null,
            TypeDefinition baseType = null,
            bool isAbstract = false,
            bool isInterface = false) {

            ClrType = clrType;
            Name = name;
            Namespace = @namespace;
            Members = (members ?? Array.Empty<MemberDefinition>()).ToList().AsReadOnly();
            GenericArguments = (genericArguments ?? Array.Empty<GenericArgumentDefinition>()).ToList().AsReadOnly();
            ImplementedInterfaces = (implementedInterfaces ?? Array.Empty<TypeDefinition>()).ToList().AsReadOnly();
            Actions = (actions ?? Array.Empty<ActionDefinition>()).ToList().AsReadOnly();
            BaseType = baseType;
            IsAbstract = isAbstract;
            IsInterface = isInterface;
            
            FullName = Helper.GetFullName(this);
            
            NamespacePath = string.IsNullOrEmpty(@namespace)
                ? Array.Empty<string>()
                : @namespace.Split('.');
                
            AsInterfaceDeclaration = baseType != null
                ? $""
                : $"";

            if (isInterface) {
                AsTypeDeclaration = AsInterfaceDeclaration;
            }
            else {
                AsTypeDeclaration = "";
            }
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
        public bool IsInterface { get; }
        
        public string FullName { get; }
        public IReadOnlyCollection<string> NamespacePath { get; }
        public string AsTypeDeclaration { get; }
        public string AsInterfaceDeclaration { get; }
        public string AsDeclaration { get; }
        public string AsReference { get; }
    }
}
