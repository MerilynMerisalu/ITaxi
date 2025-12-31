using App.BLL.DTO.AdminArea;
using Google.Apis.PeopleService.v1.Data;
using Humanizer.Localisation;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

/// <summary>
/// Details delete photo view model
/// </summary>
public class DetailsDeleteVehiclePhotoViewModel: AdminAreaBasePhotoViewModel
{
    
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