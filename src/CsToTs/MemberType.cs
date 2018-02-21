using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CsToTs {
    
    public class MemberType {

        public MemberType(MemberInfo clrMember, DataType dataType, string typeName, 
                          IEnumerable<MemberType> genericParameters = null) {
            ClrMember = clrMember;
            DataType = dataType;
            TypeName = typeName;
            GenericParameters = (genericParameters ?? Array.Empty<MemberType>()).ToList().AsReadOnly();
        }

        public MemberInfo ClrMember { get; }
        public DataType DataType { get; }
        public string TypeName { get; }
        public IReadOnlyCollection<MemberType> GenericParameters { get; }
        
        public static readonly MemberType String = new MemberType(typeof(string), DataType.String, "string");
        public static readonly MemberType Number = new MemberType(typeof(int), DataType.Number, "number");
        public static readonly MemberType Boolean = new MemberType(typeof(string), DataType.Boolean, "boolean");
        public static readonly MemberType Date = new MemberType(typeof(string), DataType.Date, "Date");
        public static readonly MemberType Any = new MemberType(typeof(object), DataType.Object, "any");
    }
}