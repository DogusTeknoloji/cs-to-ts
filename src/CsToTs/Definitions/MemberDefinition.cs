namespace CsToTs.Definitions {
    
    public class MemberDefinition {

        public MemberDefinition(string name, MemberType memberType, 
                                MemberDeclaration declaration = MemberDeclaration.GetSet) {
            Name = name;
            MemberType = memberType;
            Declaration = declaration;
        }
        
        public string Name { get; }
        public MemberType MemberType { get; }
        public MemberDeclaration Declaration { get; } 
    }
}