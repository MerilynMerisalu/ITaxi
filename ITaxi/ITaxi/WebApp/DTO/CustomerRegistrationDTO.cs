using System.ComponentModel.DataAnnotations;
using WebApp.DTO.Identity;

namespace WebApp.DTO;

public class CustomerRegistrationDTO: RegisterDTO
{
    #warning Should there be a common DTO for all types of registrations

    public Guid Id { get; set; }
    
    [Required]
    public Guid DisabilityTypeId { get; set; }
}