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
    public string FileName { get; set; } = default!;
    public string DirectoryTitleId { get; set; } = default!;
    public string FileNameInDirectory { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
       ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
       ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string? PhotoFullPath{ get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string? PhotoURL { get; set; }

    
    [MaxLength(255, ErrorMessageResourceType = typeof(Common),
       ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
       ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string? ThumbnailFullPath { get; set; } 
    public string? ThumbnailRelativePath { get; set; }


    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int OriginalPhotoHeight { get; set; }
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int OriginalPhotoWidth { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int PhotoHeight { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int PhotoWidth { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string ContentType { get; set; } = default!;

    [Range(minimum: 128, maximum: 1024, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int? ProfileImageHeight { get; set; }
    [Range(minimum: 128, maximum: 1024, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int? ProfileImageWidth { get; set; }
    public Guid? AdminId { get; set; }
    public AdminDTO? Admin { get; set; }
    public Guid? DriverId { get; set; }
    public DriverDTO? Driver { get; set; }
    
    public Guid VehicleId { get; set; }
    public VehicleDTO? Vehicle { get; set; }
    public Guid? CustomerId { get; set; }
    public CustomerDTO? Customer { get; set; }
    public Guid? AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    [NotMapped] public IFormFile? ImageFile { get; set; }
}