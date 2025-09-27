using App.BLL.DTO.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class VehicleGalleryAdminViewModel
    {
        public Guid Id { get; set; }
        public IEnumerable<PhotoDTO?>? Photos { get; set; }
        
    }
}
