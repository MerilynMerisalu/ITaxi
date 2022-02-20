using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Base.Domain.Identity;
using WebApp.Models.Enum;

namespace App.Domain.Identity;

public class AppUser : BaseUser
{
    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("First Name")]
    public string FirstName { get; set; } = default!;

    [Required]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Last Name")]
    public string LastName { get; set; } = default!;

    public string FirstAndLastName => $"{FirstName} {LastName}";
    public string LastAndFirstName => $"{LastName} {FirstName}";

    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayName("Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [DisplayName("Photo")] public Guid? PhotoId { get; set; }

    [DataType(DataType.Upload)] public Photo? Photo { get; set; }

    [Required]
    [DataType(DataType.PhoneNumber)]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Phone Number")]
    public override string PhoneNumber { get; set; } = default!;

    [Required]
    [DataType(DataType.EmailAddress)]
    [MaxLength(50)]
    [StringLength(50, MinimumLength = 1)]
    [DisplayName("Email Address")]
    public override string Email { get; set; } = default!;

    [DisplayName("Is Active")] public bool IsActive { get; set; }
}