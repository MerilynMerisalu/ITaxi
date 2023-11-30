//using System.ComponentModel.DataAnnotations;
//using Base.Contracts.Domain;

//namespace Base.Domain;

//public class Translation : Translation<Guid>, IDomainEntityId
//{
//}

//public class Translation<TKey> : DomainEntityId<TKey>
//    where TKey : IEquatable<TKey>
//{
//    [MaxLength(5)] public virtual string Culture { get; set; } = default!;

//    [MaxLength(10240)] public virtual string Value { get; set; } = " ";

//    public TKey LangStrId { get; set; } = default!;
//    public LangStr? LangStr { get; set; }
//}