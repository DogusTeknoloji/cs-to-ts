using System.Collections.Generic;
using System.Linq;
using CsToTs.Tests.Fixture;
using Xunit;

namespace CsToTs.Tests {

    public class GenerationTests {

        [Fact]
        public void ShouldHandleOpenAndClosedGenericInterface() {
            // should only generate generic type definitions
            var res = Generator.GenerateTypeScript(typeof(BaseEntity<>));
            Assert.NotEmpty(res);

            var l = new List<Company> {
                new Company { Id = 1, Name = "1" },
                new Company { Id = 2, Name = "2" },
                new Company { Id = 3, Name = "3" },
                new Company { Id = 4, Name = "4" },
                new Company { Id = 5, Name = "5" },
                new Company { Id = 6, Name = "6" }
            };
        }
    }
}