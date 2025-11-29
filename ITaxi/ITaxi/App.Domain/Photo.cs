using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Identity;
using Base.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Http;

namespace App.Domain;

public class Photo : DomainEntityMetaId
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

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoFullPath))]
    public string PhotoFullPath { get; set; } = default!;

    public string DirectoryTitleId { get; set; } = default!;
    public string FileNameInDirectory { get; set; } = default!;
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Vehicle))]
    public Guid? VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Admin))]
    public Admin? Admin { get; set; }
    public Guid? AdminId { get; set; }
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Driver))]
    public Guid? DriverId { get; set; }
    public Driver? Driver { get; set; }
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Customer))]
    public Guid? CustomerId { get; set; }
    public Customer? Customer { get; set; }
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
       ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
       ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ThumbnailFullPath))]
    public string? ThumbnailFullPath { get; set; } 
    public string? ThumbnailRelativePath { get; set; }
    [NotMapped] public IFormFile? ImageFile { get; set; }
}