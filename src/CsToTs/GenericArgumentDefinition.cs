using System;
using System.Collections.Generic;
using System.Linq;

namespace CsToTs {
    
    public class GenericArgumentDefinition {

        public GenericArgumentDefinition(string name, IEnumerable<string> typeConstraints,
                                          bool hasNewConstraint = false) {
            Name = name;
            TypeConstraints = (typeConstraints ?? Array.Empty<string>()).ToList().AsReadOnly();
            HasNewConstraint = hasNewConstraint;
        }

        public string Name { get; }
        public IReadOnlyCollection<string> TypeConstraints { get; }
        public bool HasNewConstraint { get; }
    }
}