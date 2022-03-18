using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditBookingViewModel
{
    public Guid Id { get; set; }

    [DisplayName("Vehicle Type")]
    public Guid VehicleTypeId { get; set; }

    public SelectList? VehicleTypes { get; set; }
    
    [DisplayName("City")]

    public Guid CityId { get; set; }

    public SelectList? Cities { get; set; }
    
    [DisplayName("Pickup Date and Time")]
    public DateTime PickUpDateAndTime { get; set; }

    [DisplayName("Pickup Address")]
    [StringLength(50, MinimumLength = 1)]
    public string PickupAddress { get; set; } = default!;

    [DisplayName("Destination Address")]
    [StringLength(50, MinimumLength = 1)]
    public string DestinationAddress { get; set; } = default!;
    
    [DisplayName("Number of Passengers")]
    [Range(1, 5)]
    public int NumberOfPassengers { get; set; }
    [DisplayName("Has an Assistant")]
    public bool HasAnAssistant { get; set; }
    
    [DataType(DataType.MultilineText)]
    [DisplayName("Additional Info")]
    [StringLength(1000)]
    public string? AdditionalInfo { get; set; }
}