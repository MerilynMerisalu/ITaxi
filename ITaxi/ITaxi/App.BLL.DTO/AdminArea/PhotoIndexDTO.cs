using App.BLL.DTO.Identity;
using App.Resources.Areas.App.Domain;
using Base.Domain;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace App.BLL.DTO.AdminArea
{
    public class PhotoIndexDTO : DomainEntityMetaId
    {
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Title))]
        public string Title { get; set; } = default!;
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(DirectoryTitleId))]
        public string DirectoryTitleId { get; set; } = default!;
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(FileNameInDirectory))]
        public string FileNameInDirectory { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Common),
           ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
           ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoFullPath))]
        public string? PhotoFullPath { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Common),
           ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
           ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ThumbnailFullPath))]
        public string ThumbnailFullPath { get; set; } = default!;
        /// <summary>
        /// Relative path to the thumbnail
        /// </summary>

        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ThumbnailRelativePath))]
        public string ThumbnailRelativePath { get; set; } = default!;
        /// <summary>
        /// Relative path to photo files
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [StringLength(255, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
        public string? PhotoURL { get; set; }

        

        public Guid? VehicleId { get; set; }
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Vehicle))]
        public VehicleDTO? Vehicle { get; set; }

        public Guid? AdminId { get; set; }

        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Admin))]
        public AdminDTO? Admin { get; set; }
        public Guid? DriverId { get; set; }
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Driver))]
        public DriverDTO? Driver { get; set; }

        public Guid? CustomerId { get; set; }
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Customer))]
        public CustomerDTO? Customer { get; set; }
        public Guid? AppUserId { get; set; }
        public AppUser? AppUser { get; set; }


        [NotMapped] public IFormFile? ImageFile { get; set; }
    }
}
