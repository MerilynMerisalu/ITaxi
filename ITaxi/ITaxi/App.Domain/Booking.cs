using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Booking : DomainEntityMetaId
{
    
    public Guid ScheduleId { get; set; }

    public Schedule? Schedule { get; set; }

    public Guid DriverId { get; set; }
    public Driver? Driver { get; set; }

    
    public Guid CustomerId { get; set; }

    public Customer? Customer { get; set; }
    
    public Guid VehicleTypeId { get; set; }
    public VehicleType? VehicleType { get; set; }
    public Guid VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public Guid CityId { get; set; }
    public City? City { get; set; }

    [DataType(DataType.DateTime)]
    public DateTime PickUpDateAndTime { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    public string PickupAddress { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    public string DestinationAddress { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int NumberOfPassengers { get; set; }

    public bool HasAnAssistant { get; set; }

    [MaxLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [DataType(DataType.MultilineText)]
    public string? AdditionalInfo { get; set; }
    
    public StatusOfBooking StatusOfBooking { get; set; }

    public Guid? DriveId { get; set; }
    public Drive? Drive { get; set; }

    public string DriveTime => $"{PickUpDateAndTime:g}";

    public bool IsDeclined { get; set; }
    
    public DateTime DeclineDateAndTime { get; set; }

    public string DeclineDateAndTimeCustomerView => $"{DeclineDateAndTime:g}";

    public string? ConfirmedBy { get; set; }
    
    public string? DeclinedBy { get; set; }

   
}