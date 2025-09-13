using App.BLL.DTO.AdminArea;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class VehicleGalleryAdminViewModel
    {
        public IEnumerable<PhotoDTO?>? photos { get; set; }
        public IEnumerable<string?>? ImagesRelativePathURLs = new List<string>();
        public IEnumerable<string?>? ImagesTitles { get; set; }
    }
}
