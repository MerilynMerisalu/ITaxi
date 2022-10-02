using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Base.Domain;

namespace App.Domain.DTO;

public class VehicleDTO : DomainEntityMetaId
{
    public Guid VehicleTypeId { get; set; }
    
    //public VehicleType? VehicleType { get; set; }
    
    public Guid VehicleMarkId { get; set; }
    
    public VehicleMark? VehicleMark { get; set; }
    
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleModel")]
    public Guid VehicleModelId { get; set; }
    
    [Required(/*ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage"*/)]
    [MaxLength(25)]
    [StringLength(25, MinimumLength = 1)]
    //[Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehiclePlateNumber")]
    public string VehiclePlateNumber { get; set; } = default!;

    [Required]
    public int ManufactureYear { get; set; }

    [Range(1, 6/*, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange"*/)]
    //[Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "NumberOfSeats")]
    [Required(/*ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage"*/)]
    public int NumberOfSeats { get; set; }
    
   //[Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleAvailability")]
    public VehicleAvailability VehicleAvailability { get; set; }


}