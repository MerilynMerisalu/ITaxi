using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.CustomerArea;


namespace WebApp.Areas.CustomerArea.ViewModels;

public class DeclineBookingViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;
    
    [Display(ResourceType = typeof(Booking), Name = nameof(City))]
    public string City { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(PickUpDateAndTime))]

    public string PickUpDateAndTime { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    public string? AdditionalInfo { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = nameof(StatusOfBooking))]

    public StatusOfBooking StatusOfBooking { get; set; }
}

    