using App.Enum.Enum;
using App.Resources.Areas.App.Domain.AdminArea;
using Base.Contracts.ViewModels;
using Base.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;
using System.Text;
using System.Xml.Linq;

namespace App.BLL.DTO.AdminArea
{
    public class UserManagementDTO: DomainEntityMetaId, IShowHideItem
    {
        public Guid? AdminId { get; set; }
        public Guid? DriverId { get; set; }
        public Guid? CustomerId { get; set; }
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;
        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(LastName))]
        public string LastName { get; set; } = default!;

        public string FirstAndLastName => $"{FirstName} {LastName}";
        public string LastAndFirstName => $"{LastName} {FirstName}";

        [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(Role))]
        public LangStr Role { get; set; } = default!;

        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser), Name = nameof(EmailAddress))]
        public string EmailAddress { get; set; } = default!;

        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser),
            Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;
        [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.AppUser),
            Name = nameof(Gender))]
        
        public Gender Gender { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(DateOfBirth))]
        public string DateOfBirth { get; set; } = default!;

        [Display(ResourceType = typeof(AppUser), Name = nameof(PersonalIdentifier))]
        public string? PersonalIdentifier { get; set; }
        public Guid CountryId { get; set; }
        [Display(ResourceType = typeof(AppUser), Name = nameof(Country))]
        public string Country { get; set; } = default!;

        public Guid CountyId { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(County))]
        public string County { get; set; } = default!;
        [Display(ResourceType = typeof(AppUser), Name = nameof(City))]
        public string City { get; set; } = default!;

        [Display(ResourceType = typeof(AppUser), Name = nameof(AddressOfResidence))]
        public string AddressOfResidence { get; set; } = default!;

        [Display(ResourceType = typeof(AppUser), Name = nameof(DriverLicenseCategories))]
        public string? DriverLicenseCategories { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(DriverLicenseNumber))]
        public string? DriverLicenseNumber { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(DriverLicenseExpiryDate))]
        public string? DriverLicenseExpiryDate { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(DisabilityType))]
        public string? DisabilityType { get; set; }

        [Display(ResourceType = typeof(AppUser), Name = nameof(IsActive))]
        public bool IsActive { get; set; }
    }
}

