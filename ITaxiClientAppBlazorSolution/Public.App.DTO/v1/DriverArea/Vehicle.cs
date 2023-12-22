using System.ComponentModel.DataAnnotations;
using ITaxi.Enum.Enum;
using Base.Domain;
using Base.Resources;
using Public.App.DTO.v1;
using Public.App.DTO.v1.AdminArea;

namespace Public.App.DTO.v1.DriverArea;

public class Vehicle : Entity
{
    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "Driver")]
    public Guid DriverId { get; set; }

    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = nameof(Driver))]
    public Driver? Driver { get; set; }

    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleType")]

    public Guid VehicleTypeId { get; set; }


    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleType")]

    public VehicleType? VehicleType { get; set; }


    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleMark")]

    public Guid VehicleMarkId { get; set; }


    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleMark")]

    public VehicleMark? VehicleMark { get; set; }


    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleModel")]

    public Guid VehicleModelId { get; set; }

    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleModel")]
    public VehicleModel? VehicleModel { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(25, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(25, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehiclePlateNumber")]
    public string VehiclePlateNumber { get; set; } = default!;

    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "ManufactureYear")]
    [Required]
    public int ManufactureYear { get; set; }

    [Range(2, 6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "NumberOfSeats")]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public int NumberOfSeats { get; set; }

    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleIdentifier")]
    public string VehicleIdentifier => $"{VehicleMark?.VehicleMarkName} {VehicleModel?.VehicleModelName} " +
                                       $"{VehiclePlateNumber} {VehicleType?.VehicleTypeName}";

    [Display(ResourceType = typeof(ITaxi.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleAvailability")]
    public VehicleAvailability VehicleAvailability { get; set; }

    public ICollection<Photo>? VehiclePhotos { get; set; }

}