using System.Collections.Generic;
using System.Text.RegularExpressions;
using CsToTs.Tests.Fixture;
using Xunit;

namespace CsToTs.Tests {

    public class GenerationTests {

        [Fact]
        public void ShouldGenerateOpenGenericInterface() {
            var gen = Generator.GenerateTypeScript(typeof(IBase<>));

            var ibase = GetGeneratedType(gen, "export interface IBase<T>");
            Assert.NotEmpty(ibase);
            Assert.Contains("Id: T", ibase);
        }

        [Fact]
        public void ShouldGenerateOpenForClosedGenericInterface() {
            var gen = Generator.GenerateTypeScript(typeof(IBase<int>));

            Assert.DoesNotContain("IBase<number>", gen);

            var ibase = GetGeneratedType(gen, "export interface IBase<T>");
            Assert.NotEmpty(ibase);
        }

        [Fact]
        public void ShouldGenerateOpenGenericClass() {
            var gen = Generator.GenerateTypeScript(typeof(BaseEntity<>));

            Assert.Equal(3, GetTypeCount(gen));

            var baseEntity = GetGeneratedType(gen, "export abstract class BaseEntity<TKey>");
            Assert.NotEmpty(baseEntity);
            Assert.Contains("implements IBase<TKey>", baseEntity);
            Assert.Contains("Id: TKey", baseEntity);
            Assert.Contains("CreateUser: string", baseEntity);
            Assert.Contains("CreateDate: string", baseEntity);
            Assert.Contains("UpdateUser: string", baseEntity);
            Assert.Contains("UpdateDate: string", baseEntity);
            Assert.Contains("IsActive: boolean", baseEntity);
            Assert.Contains("Type: TypeEnum", baseEntity);

            var typeEnum = GetGeneratedType(gen, "export enum TypeEnum");
            Assert.NotEmpty(typeEnum);
            Assert.Contains("Type1 = 2", typeEnum);
            Assert.Contains("Type2 = 5", typeEnum);
        }

        [Fact]
        public void ShouldGenerateOpenForClosedGenericClass() {
            var gen = Generator.GenerateTypeScript(typeof(BaseEntity<int>));

            Assert.DoesNotContain("BaseEntity<number>", gen);

            var baseEntity = GetGeneratedType(gen, "export abstract class BaseEntity<TKey>");
            Assert.NotEmpty(baseEntity);
        }

        [Fact]
        public void ShouldGenerateWithGenericConstraint() {
            var gen = Generator.GenerateTypeScript(typeof(Company<>));

            Assert.Equal(5, GetTypeCount(gen));

            var address = GetGeneratedType(gen, "export class Address");
            Assert.NotEmpty(address);
            Assert.Contains("City: string", address);
            Assert.Contains("Detail: string", address);

            var company = GetGeneratedType(gen, "export class Company<TAddress extends Address>");
            Assert.NotEmpty(company);
            Assert.Contains("Address: TAddress", company);
            Assert.Contains("extends BaseEntity<number>", company);
        }

        [Fact]
        public void ShouldGenerateWithCorrectOrder() {
            var gen = Generator.GenerateTypeScript(typeof(Company<>));
            //todo
        }

        [Fact]
        public void ShouldGenerateTypesOnlyOnce() {
            var gen = Generator.GenerateTypeScript(typeof(Company<>), typeof(Address), typeof(TypeEnum));

            Assert.Single(Regex.Matches(gen, "export class Address"));
            Assert.Single(Regex.Matches(gen, "export enum TypeEnum"));
        }

        [Fact]
        public void ShouldGenerateInterfaceForClasses() {
            var options = new TypeScriptOptions {
                UseInterfaceForClasses = true
            };
            var gen = Generator.GenerateTypeScript(typeof(Company<>), options);

            Assert.Equal(4, Regex.Matches(gen, "export interface").Count);

            var baseEntity = GetGeneratedType(gen, "export interface BaseEntity");
            Assert.NotEmpty(baseEntity);
            Assert.Contains("extends IBase<TKey>", gen);

            var company = GetGeneratedType(gen, "export interface Company");
            Assert.NotEmpty(company);
            Assert.Contains("extends BaseEntity<number>", gen);
        }

        [Fact]
        public void ShouldGenerateDateForDateTimes() {
            var gen = Generator.GenerateTypeScript(typeof(Company<>), typeof(Address), typeof(TypeEnum));
            //todo
        }

        private static string GetGeneratedType(string generated, string declaration) {
            var match = Regex.Match(generated, declaration + @".*?(export|$)", RegexOptions.Singleline);

            return match.Success ? match.Value : string.Empty;
        }

        private static int GetTypeCount(string generated) 
            => Regex.Matches(generated, @"export ", RegexOptions.Singleline).Count;
    }
}