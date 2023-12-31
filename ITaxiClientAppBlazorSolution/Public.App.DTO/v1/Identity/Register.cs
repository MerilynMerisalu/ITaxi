using Base.Resources;
using ITaxi.Enum.Enum;
using ITaxi.Resources.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.Identity
{

    /// <summary>
    /// Register DTO for the all the user types
    /// </summary>
    public class Register: Entity
    {
        /// <summary>
        /// User's first name
        /// </summary>
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// User's last name
        /// </summary>
        [Required()]
        [MaxLength(50)]
        [StringLength(50, MinimumLength = 1)]

        public string LastName { get; set; } = default!;

        /// <summary>
        /// User's gender
        /// </summary>
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        /// <summary>
        /// User's date of birth
        /// </summary>
        [Required]
        [DataType(DataType.DateTime)]
        public string DateOfBirth { get; set; } = default!;

        /// <summary>
        /// User's phone number
        /// </summary>
        [Required()]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50)]
        [StringLength(50, MinimumLength = 1)]

        public string PhoneNumber { get; set; } = default!;

        /// <summary>
        /// User's email
        /// </summary>
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Invalid email address length")]
        public string Email { get; set; } = default!;
        /// <summary>
        /// User's password
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [StringLength(100, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(AdminRegister), Name = nameof(Password))]
        public string Password { get; set; } = default!;
    }
}
