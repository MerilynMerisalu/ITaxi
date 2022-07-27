using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Base.Domain;
using Base.Resources;

namespace App.Domain;

public class Booking : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(Schedule))]
    public Schedule? Schedule { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "Driver")]

    public Guid DriverId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(Driver))]
    public Driver? Driver { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "Customer")]
    public Guid CustomerId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(Customer))]
    public Customer? Customer { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(VehicleType))]
    public VehicleType? VehicleType { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "Vehicle")]
    public Guid VehicleId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(Vehicle))]

    public Vehicle? Vehicle { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = "City")]
    public Guid CityId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(City))]
    public City? City { get; set; }

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(PickUpDateAndTime))]
    [DisplayFormat(DataFormatString = "{0:g}")]
    public DateTime PickUpDateAndTime { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(HasAnAssistant))]

    public bool HasAnAssistant { get; set; }

    [MaxLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(AdditionalInfo))]
    public string? AdditionalInfo { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Booking), Name = nameof(StatusOfBooking))]

    public StatusOfBooking StatusOfBooking { get; set; }

    public Guid? DriveId { get; set; }
    public Drive? Drive { get; set; }

    public string DriveTime => $"{PickUpDateAndTime:g}";

    public bool IsDeclined { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.Common), Name = "DeclineDateAndTime")]
    public DateTime DeclineDateAndTime { get; set; }
}