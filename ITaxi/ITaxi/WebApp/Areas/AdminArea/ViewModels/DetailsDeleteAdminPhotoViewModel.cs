using App.Resources.Areas.App.Domain.AdminArea;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class DetailsDeleteAdminPhotoViewModel: AdminAreaBasePhotoViewModel
    {
        public Guid AdminId { get; set; }
        [Display(ResourceType = typeof(Photo), Name = nameof(Admin))]
        public string Admin { get; set; } = default!;
    }
}
