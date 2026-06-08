using Base.Resources.Base.Domain;
using System.ComponentModel.DataAnnotations;
using App.Resources.Areas.App_Domain.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class UsersManagementViewModel
    {
        [Display(ResourceType = typeof(AppUser), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string FirstAndLastName => $"{FirstName} {LastName}";
        public string LastAndFirstName => $"{LastName} {FirstName}";
        public string Role { get; set; } = default!;
    }
}
