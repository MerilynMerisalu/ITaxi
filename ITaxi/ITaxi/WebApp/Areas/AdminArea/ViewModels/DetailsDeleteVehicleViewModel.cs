using System.ComponentModel;
using WebApp.Models.Enum;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteVehicleViewModel
{
    public Guid? Id { get; set; }
    [DisplayName("Driver")]
    public string DriverFullName { get; set; } = default!;

    [DisplayName("Vehicle Type")]
    public string VehicleType { get; set; } = default!;
    
    [DisplayName("Vehicle Mark")]
    public string VehicleMark { get; set; } = default!;
    
    [DisplayName("Vehicle Model")]
    public string VehicleModel { get; set; } = default!;
    
    [DisplayName("Vehicle Plate Number")]
    public string VehiclePlateNumber { get; set; } = default!;
    [DisplayName("Manufacture Year")]
    public int ManufactureYear { get; set; }

    [DisplayName("Number of Seats")]
    public int NumberOfSeats { get; set; }

    [DisplayName("Vehicle Availability")] 
    public VehicleAvailability VehicleAvailability { get; set; }





}