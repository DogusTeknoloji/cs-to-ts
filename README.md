# cs-to-ts

[![Build status](https://ci.appveyor.com/api/projects/status/mt39lws6eo92tys6?svg=true)](https://ci.appveyor.com/project/DogusTeknoloji/cs-to-ts)
[![Coverage Status](https://coveralls.io/repos/github/DogusTeknoloji/cs-to-ts/badge.svg?branch=master)](https://coveralls.io/github/DogusTeknoloji/cs-to-ts?branch=master)
[![NuGet Badge](https://buildstats.info/nuget/CsToTs)](https://www.nuget.org/packages/CsToTs/)
[![GitHub issues](https://img.shields.io/github/issues/DogusTeknoloji/cs-to-ts.svg)](https://github.com/DogusTeknoloji/cs-to-ts/issues)
[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://raw.githubusercontent.com/DogusTeknoloji/cs-to-ts/master/LICENSE)

[![GitHub stars](https://img.shields.io/github/stars/DogusTeknoloji/cs-to-ts.svg?style=social&label=Star)](https://github.com/DogusTeknoloji/cs-to-ts)
[![GitHub forks](https://img.shields.io/github/forks/DogusTeknoloji/cs-to-ts.svg?style=social&label=Fork)](https://github.com/DogusTeknoloji/cs-to-ts)

From C# to TypeScript.

```CSharp
public class Company<TAddress>: BaseEntity<int> where TAddress: Address, new() {
    public string Name { get; set; }
    public int EmployeeCount { get; set; }
    public decimal Income;
    public IList<TAddress> Address { get; set; }
}

var typeScript = CsToTs.Generator.GenerateTypeScript(typeof(Company<>));

```

Generates below TypeScript;

```TypeScript
export interface IBase<T> {
    Id: T;
}
    
export abstract class BaseEntity<TKey> implements IBase<TKey> {
    UpdateUser: string;
    Type: TypeEnum;
    Id: TKey;
    CreateUser: string;
    CreateDate: string;
    UpdateDate: string;
    IsActive: boolean;
    InternalType: any;
}
    
export class Address extends BaseEntity<number> implements IBase<number> {
    PostalCode: number;
    CompanyId: number;
    City: string;
    Detail: string;
}
    
export class Company<TAddress extends Address & { new(): TAddress }> extends BaseEntity<number> {
    Income: number;
    Name: string;
    EmployeeCount: number;
    Address: Array<TAddress>;
}

export enum TypeEnum {
    Type1 = 2,
    Type2 = 5,
}


```
