using System.Collections.Generic;

namespace CsToTs.Tests.Fixture {
    
    public class Company<TAddress>: BaseEntity<int> where TAddress: Address, new() {
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
        public decimal Income;
        public IList<TAddress> Addresses { get; set; }
        public TAddress[] AddressesArray { get; set; }
    }
}