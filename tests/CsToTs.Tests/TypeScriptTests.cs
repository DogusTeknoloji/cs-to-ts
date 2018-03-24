using System.Collections.Generic;
using CsToTs.Tests.Fixture;
using Xunit;

namespace CsToTs.Tests {

    public class GenerationTests {

        [Fact]
        public void ShouldHandleOpenAndClosedGenericInterface() {
            // should only generate generic type definitions
            var res = Generator.GenerateTypeScript(typeof(Company));
            Assert.NotEmpty(res);
        }
    }
}