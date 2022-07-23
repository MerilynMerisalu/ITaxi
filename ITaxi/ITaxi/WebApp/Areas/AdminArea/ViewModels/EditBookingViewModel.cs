﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditBookingViewModel
{
    public Guid Id { get; set; }

    [Display(ResourceType = typeof(Booking), Name = "Schedule")]
    public Guid ScheduleId { get; set; }

    [Display(ResourceType = typeof(Booking), Name = "Driver")]
    public Guid DriverId { get; set; }

    [Display(ResourceType = typeof(Booking), Name = "Customer")]
    public Guid CustomerId { get; set; }

    [Display(ResourceType = typeof(Booking), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    public SelectList? VehicleTypes { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = "City")]
    public Guid CityId { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = "Vehicle")]
    public Guid VehicleId { get; set; }
    
    public SelectList? Cities { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(PickUpDateAndTime))]
    public DateTime PickUpDateAndTime { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    [StringLength(50, MinimumLength = 1)]
    public string PickupAddress { get; set; } = default!;

    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string DestinationAddress { get; set; } = default!;
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(Booking), Name = nameof(NumberOfPassengers))]
    public int NumberOfPassengers { get; set; }
    
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]

    public bool HasAnAssistant { get; set; }

    [MaxLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    public string? AdditionalInfo { get; set; }

    public SelectList? Schedules { get; set; }

    public SelectList? Drivers { get; set; }

    public SelectList? Customers { get; set; }

    public SelectList? Vehicles { get; set; }
}