using App.Resources.Areas.App.Domain.AdminArea;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class DetailsDeleteDriverPhotoViewModel: AdminAreaBasePhotoViewModel
    {
        [Display(ResourceType = typeof(Photo), Name = nameof(Driver))]
        public string Driver { get; set; } = default!;
    }
}
