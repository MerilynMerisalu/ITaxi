using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Enum.Enum;
using Base.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Http;

namespace App.BLL.DTO.Identity;

public class AppUser: DomainEntityId
{
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(FirstName))]
    public string FirstName { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;

    public string FirstAndLastName => $"{FirstName} {LastName}";

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Admin), Name = "LastAndFirstName")]
    public string LastAndFirstName => $"{LastName} {FirstName}";

    [Display(ResourceType = typeof(Common), Name = nameof(Gender))]
    [EnumDataType(typeof(Gender))]
    public Gender Gender { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(DateOfBirth))]
    [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
#warning DateTime input control does not support user changing the language yet.
    public DateTime DateOfBirth { get; set; }

    [NotMapped]
    [Display(ResourceType = typeof(Common), Name = "ProfilePhoto")]
    public IFormFile? ProfileImage { get; set; }


    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Common), Name = nameof(PhoneNumber))]
    public  string PhoneNumber { get; set; } = default!;

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.EmailAddress)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Common), Name = "Email")]
    public  string Email { get; set; } = default!;

    [Display(ResourceType = typeof(Common), Name = nameof(IsActive))]
    public bool IsActive { get; set; }

}