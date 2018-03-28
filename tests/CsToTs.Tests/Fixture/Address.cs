namespace CsToTs.Tests.Fixture {

    public class Address: BaseEntity<int> {
        public string City { get; set; }
        public string Detail { get; set; }
    }
}