using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain;
using Base.Contracts.Domain;
namespace Base.Domain;

public abstract class DomainEntityMetaId : DomainEntityId<Guid>, IDomainEntityId, IDomainEntityMeta
{
    [Display(ResourceType = typeof(Common), Name = nameof(CreatedBy))]
    public string? CreatedBy { get; set; }
    [Display(ResourceType = typeof(Common), Name = nameof(CreatedAt))]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [Display(ResourceType = typeof(Common), Name = nameof(UpdatedBy))]
    public string? UpdatedBy { get; set; }
    [Display(ResourceType = typeof(Common), Name = nameof(UpdatedAt))]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}

public abstract class DomainEntityMetaId<TKey> : DomainEntityId<TKey>, 
    IDomainEntityMeta where TKey : IEquatable<TKey>
{
    [MaxLength(32)]
    public string? CreatedBy { get; set; } 
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    [MaxLength(32)]
    public string? UpdatedBy { get; set; } 
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}