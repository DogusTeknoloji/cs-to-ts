namespace CsToTs.TypeScript {
    
    public class MemberDefinition {

        public MemberDefinition(string name, string type) {
            Name = name;
            Type = type;
        }
        
        public string Name { get; }
        public string Type { get; }
    }
}