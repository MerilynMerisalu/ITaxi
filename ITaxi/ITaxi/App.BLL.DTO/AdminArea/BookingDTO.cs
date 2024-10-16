﻿using System.ComponentModel.DataAnnotations;

using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO.AdminArea;

public class BookingDTO: DomainEntityMetaId
{
    public Guid ScheduleId { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(Schedule))]
    public ScheduleDTO? Schedule { get; set; }
    
    public Guid DriverId { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = nameof(Driver) )]
    public DriverDTO? Driver { get; set; }
    public Guid CustomerId { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = nameof(Customer))]

    public CustomerDTO? Customer { get; set; }
    
    public Guid VehicleTypeId { get; set; }
    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleType))]
    public VehicleTypeDTO? VehicleType { get; set; }
    public Guid VehicleId { get; set; }
    [Display(ResourceType = typeof(Booking), Name = nameof(Vehicle))]
    public VehicleDTO? Vehicle { get; set; }
    public Guid CityId { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = nameof(City))]
    public CityDTO? City { get; set; }

    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Booking), Name = "PickUpDateAndTime")]
    public DateTime PickUpDateAndTime { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    public string PickupAddress { get; set; } = default!;
    [Display(ResourceType = typeof(Booking), Name = nameof(NeedAssistanceLeavingTheBuilding))]
    public bool NeedAssistanceLeavingTheBuilding { get; set; }

    [Range(minimum: 0, maximum: 35)]
    [Display(ResourceType = typeof(Booking), Name = nameof(PickupFloorNumber))]
    public int PickupFloorNumber { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnElevatorInThePickupBuilding))]
    public bool HasAnElevatorInThePickupBuilding { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = 
        "ErrorMessageMaxLength")]
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    public string DestinationAddress { get; set; } = default!;
    
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnElevatorInTheDestinationBuilding))]
    public bool HasAnElevatorInTheDestinationBuilding { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(NeedAssistanceEnteringTheBuilding))]
    public bool NeedAssistanceEnteringTheBuilding { get; set; }
    [Range(minimum: 0, maximum: 35)]
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationFloorNumber))]

    public int DestinationFloorNumber { get; set; }
    

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(Booking), Name = "NumberOfPassengers")]
    public int NumberOfPassengers { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]
    public bool HasAnAssistant { get; set; }

    [MaxLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    public string? AdditionalInfo { get; set; }
    [Display(ResourceType = typeof(Booking), Name = "StatusOfBooking")]
    public StatusOfBooking StatusOfBooking { get; set; }

    public Guid? DriveId { get; set; }
    

    public string DriveTime => $"{PickUpDateAndTime:g}";

    public bool IsDeclined { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = "BookingDeclineDateAndTime")]
    public DateTime DeclineDateAndTime { get; set; }

    public string DeclineDateAndTimeCustomerView => $"{DeclineDateAndTime:g}";

    public string? ConfirmedBy { get; set; }
    
    public string? DeclinedBy { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = "DriverInfo")]
    public string? DriverInfo => $"{Driver!.AppUser.FirstAndLastName}; {Common.PhoneNumber} {Driver.AppUser.PhoneNumber}";

    [Display(ResourceType = typeof(Booking), Name = nameof(VehicleInfo))]
    public string? VehicleInfo => $"{VehicleType.VehicleTypeName} {Vehicle.VehicleIdentifier}";
    [Display(ResourceType = typeof(Booking), Name = nameof(CustomerInfo))]
    public string? CustomerInfo => $"{Customer.AppUser.FirstAndLastName};{Common.PhoneNumber} {Customer.AppUser.PhoneNumber}";
}