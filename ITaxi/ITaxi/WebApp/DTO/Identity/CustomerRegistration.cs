﻿using System.ComponentModel.DataAnnotations;

namespace WebApp.DTO.Identity;

public class CustomerRegistration: RegisterDTO
{
    #warning Should there be a common DTO for all types of registrations
    
    [Required]
    public Guid DisabilityTypeId { get; set; }
}