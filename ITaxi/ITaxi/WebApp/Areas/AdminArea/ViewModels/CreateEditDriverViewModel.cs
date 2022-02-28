using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApp.Areas.AdminArea.ViewModels;

public class CreateEditDriverViewModel
{
    public Guid Id { get; set; }
    
    [StringLength(25)]
    [DisplayName("Personal Identifier")]
    public string? PersonalIdentifier { get; set; } 
    
    [StringLength(15, MinimumLength = 2)]
    [DisplayName("Driver License Number")]
    public string DriverLicenseNumber { get; set; } = default!;
    
    
    [DisplayName("Driver License Categories")]

    public SelectList? DriverLicenseCategories { get; set; }

    
    public ICollection<Guid>? DriverAndDriverLicenseCategories { get; set; }

    [DataType(DataType.Date)]
    [DisplayName("Driver License Expiry Date")]
    [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DriverLicenseExpiryDate { get; set; }

    public SelectList? Cities { get; set; }

    [DisplayName(nameof(City))]
    public Guid CityId { get; set; }

    [Required]
    [StringLength(30, MinimumLength = 1)]
    public string Address { get; set; } = default!;

}