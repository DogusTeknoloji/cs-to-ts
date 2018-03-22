namespace CsToTs.TypeScript {
    
    public class EnumField {

        public EnumField(string name, string value) {
            Name = name;
            Value = value;
        }
        
        public string Name { get; }
        public string Value { get; }
    }
}