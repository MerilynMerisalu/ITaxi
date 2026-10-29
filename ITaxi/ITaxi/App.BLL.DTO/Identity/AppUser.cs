using App.BLL.DTO.AdminArea;
using App.Enum.Enum;
using Base.Domain;
using Base.Resources;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(11, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(11, MinimumLength = 11, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Common), Name = nameof(PersonalIdentifierCode))]
    public string PersonalIdentifierCode { get; set; } = default!;
    [NotMapped]
    [Display(ResourceType = typeof(Common), Name = "ProfilePhoto")]
    public IFormFile? ProfileImage { get; set; }
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser), Name = "Locality")]
    public Guid LocalityId { get; set; }
    public LocalityDTO? Locality { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser), Name = "Municipality")]
    public Guid MunicipalityId { get; set; }
    public MunicipalityDTO? Municipality { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser), Name = "Country")]
    public Guid CountryId { get; set; }
    public CountryDTO? Country { get; set; }
    public Guid? CountyId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser), Name = "County")]
    public CountyDTO? County { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Common), Name = nameof(Address))]
    public string Address{ get; set; } = default!;


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