using System.Collections.Generic;
using System.Linq;

namespace CsToTs.Tests.Fixture {

    public class TestApi<T> where T: class {

        public T Get() {
            return null;
        }

        public void Save() {
        }

        public IEnumerable<T> List() {
            return Enumerable.Empty<T>();
        }
    }
}