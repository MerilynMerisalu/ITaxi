using ITaxi.Enum.Enum;
using System.ComponentModel.DataAnnotations;

namespace Webapp.ViewModels
{
    public class RegisterViewModel
    {
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public Gender? Gender { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; } = default!;
        [Phone]
        public string PhoneNumber { get; set; } = default!;
        [EmailAddress]
        public string EmailAddress { get; set; } = default!;

        [DataType(DataType.Password)]
        public string Password { get; set; } = default!;

        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; } = default!;
        public bool IsAuthenicated { get; set; } = default!;

    }
}
