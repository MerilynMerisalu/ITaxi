﻿using System.ComponentModel.DataAnnotations;
using App.Domain;
using WebApp.DTO.Identity;

namespace WebApp.DTO;

public class DriverRegistrationDTO : RegisterDTO
{
    #warning Should there be a common DTO for all types of registrations
    
    [StringLength(50)]
    public string? PersonalIdentifier { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    public Guid CityId { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(72, MinimumLength = 2)]
    public string Address { get; set; } = default!;

    public Guid DriverLicenseCategoryId { get; set; }
      
    /*public Guid[]? DriverLicenseCategoryIds { get; set; }  */

    [Required()]
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string DriverLicenseNumber { get; set; } = default!;

    [Required()] 
    [DataType(DataType.Date)] 
    public string DriverLicenseExpiryDate { get; set; } = default!;

    public List<Guid>? DriverLicenseCategories { get; set; }


}