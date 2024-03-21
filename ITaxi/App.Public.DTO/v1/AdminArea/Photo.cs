using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.BLL.DTO.AdminArea;
using App.Public.DTO.v1.Identity;
using Base.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Http;

namespace App.Public.DTO.v1.AdminArea;

public class Photo: DomainEntityMetaId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Title))]
    public string Title { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
    public string? PhotoURL { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Vehicle))]

    public Guid VehicleId { get; set; }
    public VehicleDTO? Vehicle { get; set; }

    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [NotMapped] public IFormFile? ImageFile { get; set; }
}