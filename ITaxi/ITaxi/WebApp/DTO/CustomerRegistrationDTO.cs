using System.ComponentModel.DataAnnotations;
using WebApp.DTO.Identity;

namespace WebApp.DTO;

/// <summary>
/// Customer registration DTO
/// </summary>
public class CustomerRegistrationDTO: RegisterDTO
{
    /// <summary>
    /// Id
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Disability type id for the customer registration
    /// </summary>
    [Required]
    public Guid DisabilityTypeId { get; set; }
}