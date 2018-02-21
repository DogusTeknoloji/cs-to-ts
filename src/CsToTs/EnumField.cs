using System.Reflection;

namespace CsToTs {
    
    public class EnumField {

        public EnumField(FieldInfo clrField, string name, object value) {
            ClrField = clrField;
            Name = name;
            Value = value;
        }
        
        public FieldInfo ClrField { get; }
        public string Name { get; }
        public object Value { get; }
    }
}