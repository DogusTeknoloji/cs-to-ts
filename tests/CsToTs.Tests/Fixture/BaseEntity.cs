using System;

namespace CsToTs.Tests.Fixture {
    
    public class BaseEntity<TKey>: IBase<TKey>, IEquatable<TKey> where TKey: struct {
        public TKey Id { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime UpdateDate { get; set; }
        public TypeEnum Type { get; set; }
        
        public bool Equals(TKey other) {
            return Equals(Id, other);
        }
    }
}