using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CsToTs.Definitions {
    
    public class MemberDefinition {

        public MemberDefinition(MemberInfo clrMember, string name, MemberType memberType) {
            ClrMember = clrMember;
            Name = name;
            MemberType = memberType;
        }
        
        public MemberInfo ClrMember { get; }
        public string Name { get; }
        public MemberType MemberType { get; }
    }
}