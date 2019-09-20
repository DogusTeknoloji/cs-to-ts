namespace CsToTs.Tests.Fixture {

    public class Address: BaseEntity<int> {
        public int CompanyId { get; set;}
        public string City { get; set; }
        public string Detail { get; set; }
        public long PostalCode;
        public bool? Overseas { get; set; }
    }
}