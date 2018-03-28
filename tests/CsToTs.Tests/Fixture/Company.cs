namespace CsToTs.Tests.Fixture {
    
    public class Company<TAddress>: BaseEntity<int> where TAddress: Address {
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
        public TAddress Address { get; set; }
    }
}