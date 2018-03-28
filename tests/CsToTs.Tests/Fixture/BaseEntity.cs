using System;

namespace CsToTs.Tests.Fixture {
    
    public abstract class BaseEntity<TKey>: IBase<TKey>, IEquatable<TKey> where TKey: struct {
        public TKey Id { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateUser;
        public DateTime UpdateDate { get; set; }
        public bool IsActive { get; set;}
        public TypeEnum Type;
        
        public bool Equals(TKey other) {
            return Equals(Id, other);
        }
    }
}