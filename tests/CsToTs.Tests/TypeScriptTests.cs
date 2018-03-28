using System.Collections.Generic;
using System.Text.RegularExpressions;
using CsToTs.Tests.Fixture;
using Xunit;

namespace CsToTs.Tests {

    public class TypeScriptTests {

        public TypeScriptTests() {
            TypeScriptOptions.Default.SkipTypePatterns = new[] { "InternalType" };
        }

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
            Assert.Contains("InternalType: any", baseEntity);

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
            Assert.Contains("CompanyId: number", address);
            Assert.Contains("City: string", address);
            Assert.Contains("Detail: string", address);
            Assert.Contains("PostalCode: number", address);

            var company = GetGeneratedType(gen, "export class Company");
            Assert.NotEmpty(company);
            Assert.Contains("Income: number", gen);
            Assert.Contains("Address: Array<TAddress>", company);
            Assert.Contains("extends BaseEntity<number>", company);

            var constraints = Regex.Match(company, "<.*?>").Value;
            Assert.Contains("Address", constraints);
            Assert.Contains("TAddress extends Address & { new(): TAddress }", constraints);
        }

        [Fact]
        public void ShouldGenerateWithCorrectOrder() {
            var gen = Generator.GenerateTypeScript(typeof(Company<>));

            var ibaseIdx = gen.IndexOf("export interface IBase");
            Assert.NotEqual(-1, ibaseIdx);

            var baseEntityIdx = gen.IndexOf("export abstract class BaseEntity");
            Assert.NotEqual(-1, baseEntityIdx);

            var addressIdx = gen.IndexOf("export class Address");
            Assert.NotEqual(-1, addressIdx);

            var companyIdx = gen.IndexOf("export class Company");
            Assert.NotEqual(-1, companyIdx);

            Assert.True(ibaseIdx < baseEntityIdx);
            Assert.True(baseEntityIdx < addressIdx);
            Assert.True(addressIdx < companyIdx);
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

            Assert.Equal(5, Regex.Matches(gen, "export interface").Count);

            var baseEntity = GetGeneratedType(gen, "export interface BaseEntity");
            Assert.NotEmpty(baseEntity);
            Assert.Contains("extends IBase<TKey>", gen);

            var company = GetGeneratedType(gen, "export interface Company");
            Assert.NotEmpty(company);
            Assert.Contains("extends BaseEntity<number>", gen);
        }

        [Fact]
        public void ShouldGenerateDateForDateTimes() {
            var options = new TypeScriptOptions {
                UseDateForDateTime = true
            };
            var gen = Generator.GenerateTypeScript(typeof(Company<>), options);

            var company = GetGeneratedType(gen, "export class Company");
            Assert.NotEmpty(company);
            Assert.Contains("CreateDate: Date", gen);
        }

        private static string GetGeneratedType(string generated, string declaration) {
            var match = Regex.Match(generated, declaration + @".*?(export|$)", RegexOptions.Singleline);

            return match.Success ? match.Value : string.Empty;
        }

        private static int GetTypeCount(string generated) 
            => Regex.Matches(generated, @"export ", RegexOptions.Singleline).Count;
    }
}