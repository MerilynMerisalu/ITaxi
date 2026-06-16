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
        [Display(ResourceType = typeof(AppUser), Name = nameof(Role))]
        public string Role { get; set; } = default!;

        [Display(ResourceType = typeof(AppUser), Name = nameof(EmailAddress))]
        public string EmailAddress { get; set; } = default!;
        [Display(ResourceType = typeof(AppUser), Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;


    }
}
