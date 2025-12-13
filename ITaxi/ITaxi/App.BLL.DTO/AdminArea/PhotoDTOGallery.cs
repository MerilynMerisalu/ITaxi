using Base.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace App.BLL.DTO.AdminArea
{
    public class PhotoDTOGallery
    {
        public Guid Id { get; set; }
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

        public string? VehicleType { get; set; }
        public string VehicleMark { get; set; } = default!;
        public string VehicleModel { get; set; } = default!;
        public string VehiclePlateNumber { get; set; } = default!;

        public Guid? VehicleId { get; set; }
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Vehicle))]
        public string Vehicle => $"{VehicleType} {VehicleMark} {VehicleModel} {VehiclePlateNumber}";

    }
}
