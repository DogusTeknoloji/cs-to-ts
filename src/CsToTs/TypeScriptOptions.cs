using System;

namespace CsToTs {
    
    public class TypeScriptOptions: ReflectOptions {
        private static readonly Lazy<TypeScriptOptions> _lazyDefault = new Lazy<TypeScriptOptions>();
        public static TypeScriptOptions Default => _lazyDefault.Value;

        public bool UseDateForDateTime { get; set; }
        public bool UseInterfaceForClasses { get; set; }
        public Func<Type, string> DefaultBaseType { get; set; }
    }
}