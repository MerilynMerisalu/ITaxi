using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Public.App.DTO.v1.Identity
{
    public class RegisterDriver: Register
    {
        /// <summary>
        /// Driver personal identifier
        /// </summary>
        [StringLength(50)]
        public string? PersonalIdentifier { get; set; }

        /// <summary>
        /// City id for the driver
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        public Guid CityId { get; set; }

        /// <summary>
        /// Driver address
        /// </summary>
        [Required]
        [DataType(DataType.Text)]
        [StringLength(72, MinimumLength = 2)]
        public string Address { get; set; } = default!;

        /// <summary>
        /// Driver license category id
        /// </summary>
        public Guid DriverLicenseCategoryId { get; set; }

        public Guid[]? DriverLicenseCategoryIds { get; set; }  

        /// <summary>
        /// Driver license number
        /// </summary>
        [Required()]
        [DataType(DataType.Text)]
        [StringLength(25)]
        public string DriverLicenseNumber { get; set; } = default!;

        /// <summary>
        /// Driver license expiry date
        /// </summary>
        [Required()]
        [DataType(DataType.Date)]
        public string DriverLicenseExpiryDate { get; set; } = default!;

        /// <summary>
        /// Driver license categories
        /// </summary>
        //public List<Guid>? SelectedDriverLicenseCategories { get; set; }
    }
}
