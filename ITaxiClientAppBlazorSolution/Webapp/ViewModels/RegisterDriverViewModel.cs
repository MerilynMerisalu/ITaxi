﻿using Public.App.DTO.v1.AdminArea;
using System.ComponentModel.DataAnnotations;

namespace Webapp.ViewModels
{
    public class RegisterDriverViewModel: RegisterViewModel
    {
        public string? PersonalIdentifier { get; set; }
        public City? City { get; set; }
        public string Address { get; set; } = default!;
        public string DriverLicenseNumber { get; set; } = default!;
        [DataType(DataType.Date)]
        public DateTime? DriverLicenseExpiryDate { get; set; }
        public DriverLicenseCategory? DriverLicenseCategories { get; set; }
        public List<Guid?>? SelectedDriverLicenseCategories { get; set; }
    }
}
