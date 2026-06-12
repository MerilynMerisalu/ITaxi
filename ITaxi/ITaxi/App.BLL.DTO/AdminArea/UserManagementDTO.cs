using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;

namespace App.BLL.DTO.AdminArea
{
    public class UserManagementDTO: DomainEntityMetaId
    {
        [Display(ResourceType = typeof(App.Resources.Areas.App_Domain.AdminArea.AppUser), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;
        [Display(ResourceType = typeof(App.Resources.Areas.App_Domain.AdminArea.AppUser), Name = nameof(LastName))]
        public string LastName { get; set; } = default!;

        public string FirstAndLastName => $"{FirstName} {LastName}";
        public string LastAndFirstName => $"{LastName} {FirstName}";

        [Display(ResourceType = typeof(App.Resources.Areas.App_Domain.AdminArea.AppUser), Name = nameof(Role))]
        public string Role { get; set; } = default!;
        [Display(ResourceType = typeof(Resources.Areas.App_Domain.AdminArea.AppUser), Name = nameof(Email))]
        public string Email { get; set; } = default!;

        [Display(ResourceType = typeof(Resources.Areas.App_Domain.AdminArea.AppUser),
            Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;
    }
}

