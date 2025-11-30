using App.BLL.DTO.AdminArea;
using Humanizer.Localisation;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class VehicleGalleryAdminViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string FormattedTitle => Title.Replace("_", " ");

        public string VehicleIdentifier { get; set; } = default!;
        public IEnumerable<PhotoDTO?>? Photos { get; set; }

        public IFormFile? File{ get; set; }

    }
}
