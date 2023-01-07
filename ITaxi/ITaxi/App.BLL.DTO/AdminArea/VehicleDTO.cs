using System.ComponentModel.DataAnnotations;
using App.Domain.Enum;
using Base.Domain;
using Base.Resources;

namespace App.BLL.DTO.AdminArea;

public class VehicleDTO : DomainEntityMetaId
{
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "Driver")]
    public Guid DriverId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = nameof(Driver))]
    public DriverDTO? Driver { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleType")]
    public Guid VehicleTypeId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleType")]
    public VehicleTypeDTO? VehicleType { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleMark")]
    public Guid VehicleMarkId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleMark")]
    public VehicleMarkDTO? VehicleMark { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleModel")]
    public Guid VehicleModelId { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleModel")]
    public VehicleModelDTO? VehicleModel { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(25, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(25, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehiclePlateNumber")]
    public string VehiclePlateNumber { get; set; } = default!;

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "ManufactureYear")]
    [Required]
    public int ManufactureYear { get; set; }

    [Range(1, 6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "NumberOfSeats")]
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public int NumberOfSeats { get; set; }

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleIdentifier")]
    public string VehicleIdentifier => $"{VehicleMark?.VehicleMarkName} {VehicleModel?.VehicleModelName} " +
                                       $"{VehiclePlateNumber} {VehicleType?.VehicleTypeName}";

    [Display(ResourceType = typeof(Resources.Areas.App.Domain.AdminArea.Vehicle), Name = "VehicleAvailability")]
    public VehicleAvailability VehicleAvailability { get; set; }

    public ICollection<ScheduleDTO>? Schedules { get; set; }

    public ICollection<PhotoDTO>? VehiclePhotos { get; set; }
    
}