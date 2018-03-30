using System;
using System.Collections.Generic;
using System.Reflection;
using CsToTs.TypeScript;

namespace CsToTs {
    
    public class TypeScriptOptions: ReflectOptions {
        private static readonly Lazy<TypeScriptOptions> _lazyDefault = new Lazy<TypeScriptOptions>();
        public static TypeScriptOptions Default => _lazyDefault.Value;

        public bool UseDateForDateTime { get; set; }
        public Func<Type, bool> UseInterfaceForClasses { get; set; }
        public Func<Type, string> DefaultBaseType { get; set; }
        public Func<string, string> TypeRenamer { get; set; }
        public Func<Type, (IEnumerable<string> lines, string parameters)> CtorGenerator { get; set; }
        public Func<MethodInfo, MethodDefinition, bool> ShouldGenerateMethod { get; set; }
    }
}