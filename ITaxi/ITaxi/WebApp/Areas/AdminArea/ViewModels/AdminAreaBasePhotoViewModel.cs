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
