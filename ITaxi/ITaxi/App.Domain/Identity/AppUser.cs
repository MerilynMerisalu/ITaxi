using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Base.Domain.Identity;
using Microsoft.AspNetCore.Http;
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
    [DisplayName("Last And First Name")]
    public string LastAndFirstName => $"{LastName} {FirstName}";

    [DisplayName(nameof(Gender))]
    [EnumDataType(typeof(Gender))] public Gender Gender { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    [DisplayName("Date of Birth")]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    public DateTime DateOfBirth { get; set; }

   

    [NotMapped]
    [Display(Name = "Profile Photo")]
    public IFormFile? ProfileImage{ get; set; }


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