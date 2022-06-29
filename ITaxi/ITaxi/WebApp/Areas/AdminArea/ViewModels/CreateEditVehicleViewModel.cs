using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditVehicleViewModel
{
    public Guid Id { get; set; }
   
    [DisplayName("Vehicle Type")]
    public Guid VehicleTypeId { get; set; }
    [DisplayName("Vehicle Mark")]
    public Guid VehicleMarkId { get; set; }
    [DisplayName("Vehicle Model")]
    public Guid VehicleModelId { get; set; }
    
    public SelectList? VehicleTypes { get; set; }
    public SelectList? VehicleMarks { get; set; }
    public SelectList? VehicleModels { get; set; }
    
    [Required]
    [StringLength(25, MinimumLength = 1)]
    [DisplayName("Vehicle Plate Number")]
    public string VehiclePlateNumber { get; set; } = default!;

    [DisplayName("Manufacture Year")]
    public int ManufactureYear { get; set; }

    public SelectList? ManufactureYears { get; set; }
    
    [Range(1, 6)]
    [Display(Name = "Number Of Seats")]
    [Required]
    public int NumberOfSeats { get; set; }

    [DisplayName("Vehicle Availability")]
    public VehicleAvailability VehicleAvailability { get; set; }
    
    
    
    

}