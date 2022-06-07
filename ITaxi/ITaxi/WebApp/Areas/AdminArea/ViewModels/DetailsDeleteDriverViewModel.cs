using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels;

public class DetailsDeleteDriverViewModel
{
    public Guid Id { get; set; }

    [DisplayName("Personal Identifier")]
    public string? PersonalIdentifier { get; set; } 
    
    [DisplayName("Driver License Number")]
    public string DriverLicenseNumber { get; set; } = default!;

    [DisplayName("Driver License Category Names")]
    public string? DriverLicenseCategoryNames { get; set; } = default!;
    
    [DisplayName("Driver License Expiry Date")]
    [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
    public DateTime DriverLicenseExpiryDate { get; set; }
    
    
    [DisplayName("City")] 
    public string CityName { get; set; } = default!;

    [StringLength(30, MinimumLength = 1)]
    public string Address { get; set; } = default!;


}