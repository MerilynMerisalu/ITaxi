using System.ComponentModel.DataAnnotations;
using App.BLL.DTO.AdminArea;
using App.Enum.Enum;
using Base.Domain;
using Base.Resources;

namespace App.Public.DTO.v1.CustomerArea;

public class Booking : DomainEntityMetaId
{
    // public Guid ScheduleId { get; set; }

    // [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(Schedule))]
    // public ScheduleDTO? Schedule { get; set; }
    
    // public Guid DriverId { get; set; }
    
    // [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(Driver) )]
    // public DriverDTO? Driver { get; set; }
     public Guid CustomerId { get; set; }
    
    // [Display(ResourceType = typeof(Booking), Name = nameof(Customer))]

     public CustomerDTO? Customer { get; set; }
    
    public Guid VehicleTypeId { get; set; }
    public VehicleTypeDTO? VehicleType { get; set; }
    // public Guid VehicleId { get; set; }
    // [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(Vehicle))]
    // public VehicleDTO? Vehicle { get; set; }
    public Guid CityId { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(City))]
    public CityDTO City { get; set; }

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = "PickUpDateAndTime")]
    public DateTime PickUpDateAndTime { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = 
        "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = "NumberOfPassengers")]
    public int NumberOfPassengers { get; set; }

    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.CustomerArea.Booking), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    [MaxLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [DataType(DataType.MultilineText)]
    // [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    public string? AdditionalInfo { get; set; }
    // [Display(ResourceType = typeof(Booking), Name = "StatusOfBooking")]
    // public StatusOfBooking StatusOfBooking { get; set; }

    // public Guid? DriveId { get; set; }
    // public DriveDTO? Drive { get; set; }

    public string DriveTime => $"{PickUpDateAndTime:g}";

    public bool IsDeclined { get; set; }
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Booking), Name = "BookingDeclineDateAndTime")]
    public DateTime DeclineDateAndTime { get; set; }

    public string DeclineDateAndTimeCustomerView => $"{DeclineDateAndTime:g}";

    // public string? ConfirmedBy { get; set; }
    
    // public string? DeclinedBy { get; set; }
    
}