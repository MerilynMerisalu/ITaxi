using System.ComponentModel.DataAnnotations;
using Base.Contracts.Domain;

namespace Base.Domain;

public abstract class DomainEntityMetaId : DomainEntityId<Guid>, IDomainEntityId, IDomainEntityMeta
{
    public string? CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string? UpdatedBy { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public abstract class DomainEntityMetaId<TKey> : DomainEntityId<TKey>, 
    IDomainEntityMeta where TKey : IEquatable<TKey>
{
    [MaxLength(32)]
    public string? CreatedBy { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(32)]
    public string? UpdatedBy { get; set; } 
    public DateTime UpdatedAt { get; set; }
}