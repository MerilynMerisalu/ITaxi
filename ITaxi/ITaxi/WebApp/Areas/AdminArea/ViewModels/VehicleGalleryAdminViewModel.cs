using App.BLL.DTO.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class VehicleGalleryAdminViewModel
    {
        public Guid Id { get; set; }

        public string VehicleIdentifier { get; set; } = default!;
        public IEnumerable<PhotoDTO?>? Photos { get; set; }

        public Guid VehicleImageId { get; set; }

    }
}
