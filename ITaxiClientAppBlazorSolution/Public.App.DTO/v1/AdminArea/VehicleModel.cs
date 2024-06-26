﻿using System.ComponentModel.DataAnnotations;
using Base.Resources;

namespace ITaxi.Public.DTO.v1.AdminArea;

public class VehicleModel
{
    public Guid Id { get; set; }
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.VehicleModel), Name = nameof(VehicleModelName))]
    public string VehicleModelName { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.VehicleModel), Name = "VehicleMarkName")]
    public Guid VehicleMarkId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.VehicleModel), Name = "VehicleMarkName")]
    public VehicleMark? VehicleMark { get; set; }

    public override string ToString()
    {
        return this.VehicleModelName;
    }
}