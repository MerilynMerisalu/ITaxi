using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class AdminAreaBasePhotoViewModel: AdminAreaBaseViewModel
    {
        /// <summary>
        /// Photo id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Photo title
        /// </summary>

        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Title))]
        public string Title { get; set; } = default!;

        /// <summary>
        /// Photo file name
        /// </summary>
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(FileName))]
        public string FileName { get; set; } = default!;

        /// <summary>
        /// Photo url
        /// </summary>

        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoURL))]
        public string PhotoURL { get; set; } = default!;
        /// <summary>
        /// Photo full path
        /// </summary>
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(PhotoFullPath))]
        public string PhotoFullPath { get; set; } = default!;
        /// <summary>
        /// Photo's thumbnail relative path
        /// </summary>
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ThumbnailRelativePath))]
        public string ThumbnailRelativePath { get; set; } = default!;

        /// <summary>
        /// Photo's thumbnail full path
        /// </summary>
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(ThumbnailFullPath))]
        public string ThumbnailFullPath { get; set; } = default!;

        public long OriginalBytes { get; set; }
        public long PhotoBytes { get; set; }
        public long? ThumbnailBytes { get; set; }


        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        [Display(ResourceType = typeof(Photo), Name = nameof(OriginalPhotoHeight))]
        public int OriginalPhotoHeight { get; set; }
        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        [Display(ResourceType = typeof(Photo), Name = nameof(OriginalPhotoWidth))]
        public int OriginalPhotoWidth { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        [Display(ResourceType = typeof(Photo), Name = nameof(PhotoHeight))]
        public int PhotoHeight { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [Range(minimum: 300, maximum: 4096, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        [Display(ResourceType = typeof(Photo), Name = nameof(PhotoWidth))]
        public int PhotoWidth { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Photo), Name = nameof(ContentType))]
        public string ContentType { get; set; } = default!;

        [Range(minimum: 128, maximum: 1024, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        [Display(ResourceType = typeof(Photo), Name = nameof(ProfileImageHeight))]
        public int? ProfileImageHeight { get; set; }
        [Range(minimum: 128, maximum: 1024, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
        [Display(ResourceType = typeof(Photo), Name = nameof(ProfileImageWidth))]
        public int? ProfileImageWidth { get; set; }

        /// <summary>
        /// Photo's directory id
        /// </summary>
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(DirectoryTitleId))]
        public string DirectoryTitleId { get; set; } = default!;

        /// <summary>
        /// Photo's file name in directory
        /// </summary>
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(FileNameInDirectory))]
        public string FileNameInDirectory { get; set; } = default!;
        /// <summary>
        /// Photo
        /// </summary>
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "PhotoName")]
        public IFormFile? Photo { get; set; }

    }
}
