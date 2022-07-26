using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.CustomerArea;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.CustomerArea.ViewModels;

public class CreateBookingViewModel
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    public SelectList? VehicleTypes { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = "City")]
    public Guid CityId { get; set; }

    public SelectList? Cities { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = nameof(PickUpDateAndTime))]
    public DateTime PickUpDateAndTime { get; set; }

    [Display(ResourceType = typeof(Booking), Name = nameof(PickupAddress))]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string PickupAddress { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [Display(ResourceType = typeof(Booking), Name = nameof(DestinationAddress))]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "StringLengthAttributeErrorMessage" )]
    public string DestinationAddress { get; set; } = default!;
    
    [Display(ResourceType = typeof(Booking), Name = nameof(NumberOfPassengers))]
    [Range(1, 5, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    public int NumberOfPassengers { get; set; }
    [Display(ResourceType = typeof(Booking), Name = nameof(HasAnAssistant))]

    public bool HasAnAssistant { get; set; }
    
    [DataType(DataType.MultilineText)]
    [Display(ResourceType = typeof(Booking), Name = nameof(AdditionalInfo))]
    [StringLength(1000, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    public string? AdditionalInfo { get; set; }
}