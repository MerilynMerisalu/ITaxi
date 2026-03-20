using App.Domain.Identity;

using Base.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.AccessControl;
using System.Xml.Linq;

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
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(FileName))]
    public string FileName { get; set; } = default!;
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoType))]
    public PhotoType PhotoType { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
    public string PhotoURL { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoFullPath))]
    public string PhotoFullPath { get; set; } = default!;
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum:4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(OriginalPhotoHeight))]
    public int OriginalPhotoHeight { get; set; }
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(OriginalPhotoWidth))]
    public int OriginalPhotoWidth { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoHeight))]
    public int PhotoHeight { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "ProfilePhotoWidth")]
    public int PhotoWidth { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ContentType))]
    public string ContentType { get; set; } = default!;
    
    [Range(minimum: 128, maximum:1024, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "ProfilePhotoHeight")]
    public int? ProfilePhotoHeight { get; set; }
    [Range(minimum: 128, maximum: 1024, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ProfilePhotoWidth))]
    public int? ProfilePhotoWidth { get; set; }
    public string DirectoryTitleId { get; set; } = default!;
    public string FileNameInDirectory { get; set; } = default!;
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(OriginalBytes))]
    public long OriginalBytes { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoBytes))]
    public long PhotoBytes { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ThumbnailBytes))]
    public long? ThumbnailBytes { get; set; }


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