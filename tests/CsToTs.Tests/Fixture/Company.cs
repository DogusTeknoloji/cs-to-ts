namespace CsToTs.Tests.Fixture {
    
    public class Company: BaseEntity<int> {
        public string Name { get; set; }
        public int EmployeeCount { get; set; }
    }
}