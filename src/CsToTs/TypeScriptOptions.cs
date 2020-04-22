using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CsToTs.TypeScript;

namespace CsToTs {
    
    public class TypeScriptOptions {
        private static readonly string[] DefaultSkipTypePatterns = {
            @"^System\.*"
        };
        private static readonly Lazy<TypeScriptOptions> _lazyDefault = new Lazy<TypeScriptOptions>();
        public static TypeScriptOptions Default => _lazyDefault.Value;

        private IEnumerable<string> _skipTypePatterns = DefaultSkipTypePatterns;
        public IEnumerable<string> SkipTypePatterns {
            get => _skipTypePatterns;
            set => _skipTypePatterns = value == null
                ? DefaultSkipTypePatterns
                : _skipTypePatterns.Union(value);
        }

        public bool UseDateForDateTime { get; set; }
        public bool UseStringsForEnums { get; set; }
        public Func<Type, bool> UseInterfaceForClasses { get; set; }
        public Func<Type, string> DefaultBaseType { get; set; }
        public Func<string, string> TypeRenamer { get; set; }
        public Func<MemberInfo, string> MemberRenamer { get; set; }
        public Func<Type, CtorDefinition> CtorGenerator { get; set; }
        public Func<MethodInfo, MethodDefinition, bool> ShouldGenerateMethod { get; set; }
        public Func<MemberInfo, IEnumerable<string>> UseDecorators { get; set; }
    }
}