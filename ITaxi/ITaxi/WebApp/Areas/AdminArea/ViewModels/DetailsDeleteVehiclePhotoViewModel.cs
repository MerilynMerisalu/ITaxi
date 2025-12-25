using App.BLL.DTO.AdminArea;
using Google.Apis.PeopleService.v1.Data;
using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete photo view model
/// </summary>
public class DetailsDeleteVehiclePhotoViewModel: AdminAreaBaseViewModel
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
    /// Vehicle Id
    /// </summary>
    public Guid? VehicleId { get; set; }
    /// <summary>
    /// Vehicle identifier
    /// </summary>
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = nameof(Vehicle))]
    public string? Vehicle{ get; set; }

    public Guid DriverId { get; set; }
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "Driver")]
    public string? VehicleDriver { get; set; }
    /// <summary>
    /// Is vehicle boolean value
    /// </summary>
    public bool IsVehicle { get; set; }
    /// <summary>
    /// Admin's full name
    /// </summary>
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Photo), Name = "PhotoName")]
    public string? Photo { get; set; }
}