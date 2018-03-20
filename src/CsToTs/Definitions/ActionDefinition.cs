using System.Reflection;

namespace CsToTs.Definitions {
    
    public class ActionDefinition {
        
        public ActionDefinition(MethodInfo clrMethod) {
        }
        
        public static string Name { get; }
    }
}