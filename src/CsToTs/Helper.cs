using System;

namespace CsToTs {
    
    internal static class Helper {
     
        
        internal static string GetTypeName(Type type) {
            var idx = type.Name.IndexOf('`');
            return idx == -1 ? type.Name : type.Name.Substring(0, idx);
        }
    }
}