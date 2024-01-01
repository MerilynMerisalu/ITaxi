using ITaxi.Enum.Enum;
using Public.App.DTO.v1.AdminArea;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Webapp.ViewModels
{
    public class RegisterCustomerViewModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public Gender? Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = default!;
        public DisabilityType? DisabilityType { get; set; }
        
        [Phone]
        public string PhoneNumber { get; set; } = default!;
        [EmailAddress]
        public string EmailAddress { get; set;} = default!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = default!;

        public bool IsAuthenicated { get ; set; } = default!;
    }
}
