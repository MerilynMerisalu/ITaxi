using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteBookingViewModel: AdminAreaBaseViewModel
{
    public Guid Id { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = "Schedule")]

    public string ShiftDurationTime { get; set; } = default!;
    
    [Display(ResourceType = typeof(Booking), Name = nameof(Driver))]

    public string Driver { get; set; } = default!;
    
    [Display(ResourceType = typeof(Booking), Name = nameof(Customer))]

    public string Customer { get; set; } = default!;
    
    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleType))]
    public string VehicleType { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(Vehicle))]
    public string Vehicle { get; set; } = default!;
    
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