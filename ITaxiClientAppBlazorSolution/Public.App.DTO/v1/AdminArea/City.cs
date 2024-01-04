﻿using System.ComponentModel.DataAnnotations;
using Base.Resources;

namespace Public.App.DTO.v1.AdminArea;

public class City: Entity
{
    
    public Guid CountyId { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
    public string CityName { get; set; } = default!;
}