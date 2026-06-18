using Base.Resources.Base.Domain;
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App.Domain.AdminArea;


namespace WebApp.Areas.AdminArea.ViewModels
{
    public class UserManagementViewModel: AdminAreaBaseViewModel
    {
        
        [Display(ResourceType = typeof(AppUser), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;
        [Display(ResourceType = typeof(AppUser), Name = nameof(LastName))]
        public string LastName { get; set; } = default!;
        [Display(ResourceType = typeof(AppUser), Name = nameof(FirstAndLastName))]
        public string FirstAndLastName => $"{FirstName} {LastName}";
        public string LastAndFirstName => $"{LastName} {FirstName}";

        [Display(ResourceType = typeof(AppUser), Name = nameof(Gender))]
        public string Gender { get; set; } = default!;
        [Display(ResourceType = typeof(AppUser), Name = nameof(DateOfBirth))]
        public string DateOfBirth { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(PersonalIdentifier))]
        public string PersonalIdentifier { get; set; }
        [Display(ResourceType = typeof(AppUser), Name = nameof(Country))]
        public string Country { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(County))]
        public string County { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(AddressOfResidence))]
        public string AddressOfResidence { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(Role))]
        public string Role { get; set; } = default!;

        [Display(ResourceType = typeof(AppUser), Name = nameof(EmailAddress))]
        public string EmailAddress { get; set; } = default!;
        [Display(ResourceType = typeof(AppUser), Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;


    }
}
