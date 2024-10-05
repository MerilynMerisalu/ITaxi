using System.ComponentModel.DataAnnotations;
using App.Enum.Enum;
using Base.Domain;
using Base.Resources;

namespace App.DAL.DTO.AdminArea;

public class VehicleDTO : DomainEntityMetaId
{
    public Guid DriverId { get; set; }
    public DriverDTO? Driver { get; set; }
    public Guid VehicleTypeId { get; set; }
    public VehicleTypeDTO? VehicleType { get; set; }
    public Guid VehicleMarkId { get; set; }
    public VehicleMarkDTO? VehicleMark { get; set; }
    public Guid VehicleModelId { get; set; }
    public VehicleModelDTO? VehicleModel { get; set; }

    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    [MaxLength(25, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
    [StringLength(25, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
        ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
    public string VehiclePlateNumber { get; set; } = default!;

    [Required]
    public int ManufactureYear { get; set; }

    [Range(1, 6, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageRange")]
    
    [Required(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
    public int NumberOfSeats { get; set; }
    public string VehicleIdentifier => $"{VehicleMark?.VehicleMarkName} {VehicleModel?.VehicleModelName} " +
                                       $"{VehiclePlateNumber} {VehicleType?.VehicleTypeName}";
    
    [Display(ResourceType = typeof(App.Resources.Areas.App.Domain.AdminArea.Vehicle), Name = nameof(DoElectricWheelchairFitInCar))]
    public bool DoElectricWheelchairFitInCar { get; set; }
    
    public VehicleAvailability VehicleAvailability { get; set; }

    public ICollection<ScheduleDTO>? Schedules { get; set; }

    public ICollection<PhotoDTO>? VehiclePhotos { get; set; }
    
}