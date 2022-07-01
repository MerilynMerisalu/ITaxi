using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.Domain.Enum;
using Base.Domain.Identity;
using Microsoft.AspNetCore.Http;
using Base.Resources;
namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(FirstName))]
    public string FirstName { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(LastName))]
    public string LastName { get; set; } = default!;

    public string FirstAndLastName => $"{FirstName} {LastName}";
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = "LastAndFirstName")]
    public string LastAndFirstName => $"{LastName} {FirstName}";

    [Display(ResourceType = typeof(Common), Name = nameof(Gender))]
    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Admin), Name = nameof(DateOfBirth))]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

    [NotMapped]
    [Display(Name = "Profile Photo")]
    public IFormFile? ProfileImage{ get; set; }


    [Required]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [Display(ResourceType = typeof(Common), Name = nameof(PhoneNumber))]
    public override string PhoneNumber { get; set; } = default!;

    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Email Address")]
    public override string Email { get; set; } = default!;

    [Display(ResourceType = typeof(Common), Name=nameof(IsActive))] public bool IsActive { get; set; }
}