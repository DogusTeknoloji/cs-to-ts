using System.Collections.Generic;

namespace CsToTs.Tests.Fixture
{
    class SubclassOfDictionary : Dictionary<string, string>
    {
    }

    class ClassWithSubclassOfDictionary
    {
        public SubclassOfDictionary AProperty { get; set; }
    }
}