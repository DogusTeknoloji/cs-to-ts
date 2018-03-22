using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs {
    
    public class ReflectOptions {
        private static readonly string[] DefaultSkipTypePatterns = {
            @"^System\.*"
        };
        private static readonly Lazy<ReflectOptions> _lazyDefault = new Lazy<ReflectOptions>();
        public static ReflectOptions Default => _lazyDefault.Value;
        
        private IEnumerable<string> _skipTypePatterns = DefaultSkipTypePatterns;

        public IEnumerable<string> SkipTypePatterns {
            get => _skipTypePatterns;
            set => _skipTypePatterns = value == null
                ? DefaultSkipTypePatterns
                : _skipTypePatterns.Union(value);
        }
    }
}