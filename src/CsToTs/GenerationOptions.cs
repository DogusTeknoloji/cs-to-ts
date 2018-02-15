using System.Collections.Generic;
using System.Linq;

namespace CsToTs {
    
    public class GenerationOptions {
        private static readonly string[] DefaultSkipTypePatterns = {
            @"System\.*"
        };

        public GenerationOptions(IEnumerable<string> skipTypePatterns = null) {
            
        }
        
        internal bool DateForDateTime { get; private set; }

        public GenerationOptions UseDateForDateTime(bool value = true) {
            this.DateForDateTime = value;
            return this;
        }

        internal IEnumerable<string> SkipTypePatterns { get; private set; } = DefaultSkipTypePatterns;

        public GenerationOptions WithSkipTypePatterns(IEnumerable<string> patterns) {
            if (patterns == null) {
                SkipTypePatterns = DefaultSkipTypePatterns;
                return this;
            }

            SkipTypePatterns = patterns.Union(DefaultSkipTypePatterns);
            return this;
        }
        
        public static GenerationOptions Default { get; } = new GenerationOptions(); 
    }
}