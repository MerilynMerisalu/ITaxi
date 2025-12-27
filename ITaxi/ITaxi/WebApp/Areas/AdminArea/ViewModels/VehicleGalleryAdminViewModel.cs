using App.BLL.DTO.AdminArea;
using Humanizer.Localisation;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class VehicleGalleryAdminViewModel
    {
        public Guid Id { get; set; }

        public Guid VehicleId { get; set; }
        public string VehicleType { get; set; } = default!;
        public string VehicleMark { get; set; } = default!;
        public string VehicleModel { get; set; } = default!;
        public string VehiclePlateNumber { get; set; } = default!;
        public string VehicleIdentifier => $"{VehicleType} {VehicleMark} {VehicleModel} {VehiclePlateNumber}";
        public IEnumerable<PhotoDTOGallery?>? Photos { get; set; }

        public IFormFile? File{ get; set; }

    }
}
