using System.ComponentModel.DataAnnotations;
using App.Public.DTO.v1.AdminArea;
using Base.Resources;
using WebApp.DTO.Identity;

namespace WebApp.DTO;

/// <summary>
/// Driver registration DTO
/// </summary>
public class DriverRegistrationDTO : RegisterDTO
{
    
    /// <summary>
    /// Driver license category id
    /// </summary>
    public Guid DriverLicenseCategoryId { get; set; }
      
    public Guid[]? DriverLicenseCategoryIds { get; set; }  

    /// <summary>
    /// Driver license number
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))]
    [DataType(DataType.Text)]
    [StringLength(25)]
    public string DriverLicenseNumber { get; set; } = default!;

    /// <summary>
    /// Driver license expiry date
    /// </summary>
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = nameof(Common.RequiredAttributeErrorMessage))] 
    [DataType(DataType.Date)] 
    public string DriverLicenseExpiryDate { get; set; } = default!;

    /// <summary>
    /// Driver license categories
    /// </summary>
    //public List<Guid>? DriverLicenseCategories { get; set; }
}