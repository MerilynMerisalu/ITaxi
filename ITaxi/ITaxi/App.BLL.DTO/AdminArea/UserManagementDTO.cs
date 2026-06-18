using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
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
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(LastName))]
        public string LastName { get; set; } = default!;

        public string FirstAndLastName => $"{FirstName} {LastName}";
        public string LastAndFirstName => $"{LastName} {FirstName}";

        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(Role))]
        public string Role { get; set; } = default!;
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(EmailAddress))]
        public string EmailAddress { get; set; } = default!;

        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser),
            Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;
        public Gender Gender { get; set; }

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
    }
}

