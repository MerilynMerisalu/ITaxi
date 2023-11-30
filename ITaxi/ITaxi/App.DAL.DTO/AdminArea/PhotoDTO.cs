using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.DAL.DTO.Identity;
using Base.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Http;

namespace App.DAL.DTO.AdminArea;

public class PhotoDTO : DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string Title { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string? PhotoURL { get; set; }
    public Guid VehicleId { get; set; }
    public VehicleDTO? Vehicle { get; set; }

    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [NotMapped] public IFormFile? ImageFile { get; set; }
}