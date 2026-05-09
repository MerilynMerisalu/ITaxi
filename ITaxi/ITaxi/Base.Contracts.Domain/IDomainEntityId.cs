

using System.ComponentModel.DataAnnotations;
using Base.Resources.Base.Domain;


namespace Base.Contracts.Domain;

/// <summary>
///     Guid based domain entity id
/// </summary>
public interface IDomainEntityId : IDomainEntityId<Guid>
{
}

/// <summary>
///     Universal Domain Entity interface based on generic PK type
/// </summary>
/// <typeparam name="TKey">Type for primary key</typeparam>
public interface IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; }
    [Display(ResourceType = typeof(Common), Name = nameof(IsDeleted))]
    public bool IsDeleted { get; set; }
   public bool IsIgnored { get; set; }
}