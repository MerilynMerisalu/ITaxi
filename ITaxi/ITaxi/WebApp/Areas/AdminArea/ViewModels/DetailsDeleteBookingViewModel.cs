using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using WebApp.Models.Enum;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteBookingViewModel
{
    public Guid Id { get; set; }
    
    [DisplayName("Shift Duration Time")]

    public string ShiftDurationTime { get; set; } = default!;
    
    [DisplayName("Last and First Name")]

    public string LastAndFirstName { get; set; } = default!;
    
    [DisplayName("Vehicle Type")]
    public string VehicleType { get; set; } = default!;

    public string Driver { get; set; } = default!;
    public string Vehicle { get; set; } = default!;

    public string City { get; set; } = default!;

    [DisplayName("Pickup Date and Time")]
    [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
    public string PickUpDateAndTime { get; set; } = default!;
    
    [DisplayName("Pickup Address")]
    public string PickupAddress { get; set; } = default!;
    
    [DisplayName("Destination Address")]
    public string DestinationAddress { get; set; } = default!;
    
    [DisplayName("Number of Passengers")]
    public int NumberOfPassengers { get; set; }

    [DisplayName("Has an Assistant")]
    public bool HasAnAssistant { get; set; }
    [DisplayName("Additional Info")]
    public string? AdditionalInfo { get; set; }
    
    [DisplayName("Status of Booking")]

    public StatusOfBooking StatusOfBooking { get; set; }

}