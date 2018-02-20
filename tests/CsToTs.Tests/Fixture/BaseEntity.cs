using System;

namespace CsToTs.Tests.Fixture {
    
    public class BaseEntity<T>: IBase<T>, IEquatable<T> where T: struct {
        public T Id { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        
        public bool Equals(T other) {
            return Equals(Id, other);
        }
    }
}