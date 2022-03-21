using System.ComponentModel;
using App.Domain;
using WebApp.Models.Enum;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDriveViewModel
{
    [DisplayName(nameof(App.Domain.Schedule))]
    public string Schedule { get; set; } = default!;
    
    [DisplayName("Shift Duration Time")]
    public string ShiftDurationTime { get; set; } = default!;

    [DisplayName(nameof(Customer))] 
    public string LastAndFirstName { get; set; } = default!;

    [DisplayName("Pickup Date and Time")]
    public string PickupDateAndTime { get; set; } = default!;
    
    [DisplayName(nameof(City))]
    public string City { get; set; } = default!;

    [DisplayName("Pickup Address")]
    public string PickupAddress { get; set; } = default!;
    
    [DisplayName("Destination Address")]
    public string DestinationAddress { get; set; } = default!;
    
    [DisplayName("Vehicle Type")]
    public string VehicleType{ get; set; } = default!;

    [DisplayName(nameof(Vehicle))]
    public string VehicleIdentifier { get; set; } = default!;
    
    [DisplayName("Number of Passengers")]
    public string NumberOfPassengers { get; set; } = default!;
    
    [DisplayName("Has an Assistant")]
    public string HasAnAssistant { get; set; } = default!;
    
    [DisplayName("Status of Booking")]
    public StatusOfBooking StatusOfBooking { get; set; }

    [DisplayName(nameof(Comment))]
    public string CommentText { get; set; } = default!;
}