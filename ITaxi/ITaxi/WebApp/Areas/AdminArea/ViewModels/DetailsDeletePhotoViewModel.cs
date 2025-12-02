using App.BLL.DTO.AdminArea;
using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete photo view model
/// </summary>
public class DetailsDeletePhotoViewModel: AdminAreaBaseViewModel
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
    /// Vehicle Identifier
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "Vehicle")]
    public string? VehicleIdentifier{ get; set; }

    /// <summary>
    /// Vehicle
    /// </summary>
    public VehicleDTO? Vehicle { get; set; }
    /// <summary>
    /// Admin's full name
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "Admin")]
    public string? AdminFirstAndLastName { get; set; }
    /// <summary>
    /// Admin
    /// </summary>
    public AdminDTO? Admin { get; set; }
    /// <summary>
    /// Driver's full name
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "Driver")]
    public string? DriverFirstAndLastName { get; set; }
    /// <summary>
    /// Driver
    /// </summary>
    public DriverDTO? Driver { get; set; }

    /// <summary>
    /// Customer's full name
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "Customer")]
    public string? CustomerFirstAndLastName { get; set; }
    /// <summary>
    /// Customer
    /// </summary>
    public CustomerDTO? Customer { get; set; }
}