using System.Linq;
using CsToTs.Tests.Fixture;
using Xunit;

namespace CsToTs.Tests {
    
    public class ReflectionTests {

        [Fact]
        public void ShouldHandleOpenAndClosedGenericInterface() {
            // should only generate generic type definitions
            foreach (var t in new[] {typeof(IBase<>), typeof(IBase<int>)}) {
                var res = Generator.GetTypeDefinitions(t).ToList();
                Assert.Equal(1, res.Count());

                var ibase = res.First();
                Assert.Equal(1, ibase.GenericArguments.Count);

                var gen = ibase.GenericArguments.First();
                Assert.False(gen.HasNewConstraint);
                Assert.Equal("T", gen.Name);
                Assert.Equal(0, gen.TypeConstraints.Count);

                var mem = ibase.Members;
                Assert.Equal(1, mem.Count);

                var id = mem.First();
                Assert.Equal("Id", id.Name);
                Assert.Equal("T", id.MemberType.TypeName);
                Assert.Equal(DataType.Object, id.MemberType.DataType);
                Assert.Equal(0, id.MemberType.GenericParameters.Count);
                Assert.Equal(MemberDeclaration.GetSet, id.Declaration);
            }
        }
        
        [Fact]
        public void ShouldHandleOpenAndClosedGenericType() {
            // should only generate generic type definitions
            foreach (var t in new[] {typeof(BaseEntity<>), typeof(BaseEntity<int>)}) {
                var res = Generator.GetTypeDefinitions(t).ToList();
                Assert.Equal(2, res.Count());

                var bas = res.First();
                Assert.Equal(1, bas.GenericArguments.Count);

                var gen = bas.GenericArguments.First();
                Assert.False(gen.HasNewConstraint);
                Assert.Equal("T", gen.Name);
                Assert.Equal(0, gen.TypeConstraints.Count);

                var inter = bas.ImplementedInterfaces;
                Assert.Equal(1, inter.Count);

                var ibase = inter.First();
                Assert.Equal("IBase", ibase.Name);
                
                var mem = bas.Members;
                Assert.Equal(1, mem.Count);

                var id = mem.First();
                Assert.Equal("Id", id.Name);
                Assert.Equal("T", id.MemberType.TypeName);
                Assert.Equal(DataType.Object, id.MemberType.DataType);
                Assert.Equal(0, id.MemberType.GenericParameters.Count);
                Assert.Equal(MemberDeclaration.GetSet, id.Declaration);
            }
        }
    }
}