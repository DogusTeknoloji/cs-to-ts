using System.Collections.Generic;

namespace CsToTs.Tests.Fixture
{
    public class EntityWithCollections
    {
        public IEnumerable<string> IEnumerableOfStrings { get; set; }
        public IDictionary<string, int> IDictionaryOfStringsToNumbers { get; set; }
        public Dictionary<string, int> DictionaryOfStringsToNumbers { get; set; }
    }
}