using System;
using HandlebarsDotNet;

namespace CsToTs {
    
    internal class SimpleEncoder : ITextEncoder {
        static readonly Lazy<SimpleEncoder> _instance = new Lazy<SimpleEncoder>(() => new SimpleEncoder());

        private SimpleEncoder() { }

        public static SimpleEncoder Instance => _instance.Value;

        string ITextEncoder.Encode(string value) => value;
    }
}