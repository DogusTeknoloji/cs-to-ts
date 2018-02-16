using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs {
    
    public class MemberType {

        public MemberType(Type clrType, DataType dataType, string typeName, 
                          IEnumerable<MemberType> genericParameters = null) {
            ClrType = clrType;
            DataType = dataType;
            TypeName = typeName;
            GenericParameters = (genericParameters ?? Array.Empty<MemberType>()).ToList().AsReadOnly();
        }

        public Type ClrType { get; }
        public DataType DataType { get; }
        public string TypeName { get; }
        public IReadOnlyCollection<MemberType> GenericParameters { get; }
        
        public static MemberType String = new MemberType(typeof(string), DataType.String, "string");
        public static MemberType Number = new MemberType(typeof(int), DataType.Number, "number");
        public static MemberType Boolean = new MemberType(typeof(string), DataType.Boolean, "boolean");
        public static MemberType Date = new MemberType(typeof(string), DataType.Date, "Date");
        public static MemberType Any = new MemberType(typeof(object), DataType.Object, "any");
    }
}