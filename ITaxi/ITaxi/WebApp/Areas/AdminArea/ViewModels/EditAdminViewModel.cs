using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using App.Domain.Enum;
using App.Domain.Identity;
using Base.Resources;
using Microsoft.AspNetCore.Mvc.Rendering;
using Admin = App.Resources.Areas.App.Domain.AdminArea.Admin;

namespace WebApp.Areas.AdminArea.ViewModels;

public class EditAdminViewModel
{
    public Guid Id { get; set; }
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = "FirstName")]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMinMax")]
    [Display(ResourceType = typeof(Admin), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;
    
    [Display(ResourceType = typeof(Common), Name = "Gender")]
    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }

    [Display(ResourceType = typeof(Admin), Name = "DateOfBirth")]

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [StringLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [Display(ResourceType = typeof(Admin), Name = "PersonalIdentifier")]
    public string? PersonalIdentifier { get; set; }

    [Display(ResourceType = typeof(Admin), Name = "City")] public Guid CityId { get; set; }

    [StringLength(50, MinimumLength = 1)]
    public string Address { get; set; } = default!;
    
    [Phone]
    public string PhoneNumber { get; set; } = default!;

    public SelectList? Cities { get; set; }

}