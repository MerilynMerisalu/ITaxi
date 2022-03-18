using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditBookingViewModel
{
    public Guid Id { get; set; }
    
    [DisplayName("Vehicle Type")]
    
    public Guid VehicleTypeId { get; set; }

    public SelectList? VehicleTypes { get; set; }
    [DisplayName(nameof(City))]
    public Guid CityId { get; set; }

    public SelectList? Cities { get; set; }
    
    [DisplayName("Pickup Date and Time")]
    public DateTime PickUpDateAndTime { get; set; }

    [DisplayName("Pickup Address")]
    [StringLength(50, MinimumLength = 1)]
    public string PickupAddress { get; set; } = default!;
    
    [Required]
    [StringLength(50)]
    [DisplayName("Destination Address")]
    public string DestinationAddress { get; set; } = default!;
    
    [Required]
    [Range(1, 5)]
    [DisplayName("Number Of Passengers")]
    public int NumberOfPassengers { get; set; }
    
    [DisplayName("Has an Assistant?")]
    public bool HasAnAssistant { get; set; }
    
    [DataType(DataType.MultilineText)]
    [DisplayName("Additional Info")]
    public string? AdditionalInfo { get; set; }

}