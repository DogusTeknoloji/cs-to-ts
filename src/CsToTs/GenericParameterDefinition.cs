namespace CsToTs {
    
    public class GenericParameterDefinition {

        public GenericParameterDefinition(string name, bool hasNewConstraint = false) {
            Name = name;
            HasNewConstraint = hasNewConstraint;
        }
        
        public string Name { get; }
        public bool HasNewConstraint { get; }
    }
}