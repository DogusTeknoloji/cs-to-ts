using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs.Definitions {
    
    public class MemberType {
        
        public MemberType(Type clrType, TypeCode typeCode, bool isEnumerable,
                          TypeDefinition typeDefinition, IEnumerable<MemberType> genericArguments) {
            ClrType = clrType;
            TypeCode = typeCode;
            IsEnumerable = isEnumerable;
            TypeDefinition = typeDefinition;
            GenericArguments = (genericArguments ?? Enumerable.Empty<MemberType>()).ToList().AsReadOnly();
        }

        public Type ClrType { get; }
        public bool IsEnumerable { get; }
        public TypeCode TypeCode { get; }
        public TypeDefinition TypeDefinition { get; }
        public IReadOnlyCollection<MemberType> GenericArguments { get; }
    }
}