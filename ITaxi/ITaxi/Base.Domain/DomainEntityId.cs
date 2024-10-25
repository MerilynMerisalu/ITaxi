using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;
using Base.Resources;

namespace Base.Domain;

public abstract class DomainEntityId : DomainEntityId<Guid>, IDomainEntityId
{
}

public abstract class DomainEntityId<TKey> : IDomainEntityId<TKey>
    where TKey : IEquatable<TKey>
{
    public TKey Id { get; set; } = default!;
    
    
    public bool IsDeleted { get; set; } = false;
    [Display(ResourceType = typeof(Common), Name = nameof(IsIgnored))]
    public bool IsIgnored { get; set; } = false;

    public override bool Equals(object? obj)
    {
        return obj is DomainEntityId<TKey> dom && dom.Id.Equals(this.Id);
    }
    public override int GetHashCode()
    {
        return this.Id.GetHashCode();
    }
}