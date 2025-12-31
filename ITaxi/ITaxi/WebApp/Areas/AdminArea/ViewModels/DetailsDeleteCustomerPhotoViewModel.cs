using App.Resources.Areas.App.Domain.AdminArea;
using System.ComponentModel.DataAnnotations;

namespace WebApp.Areas.AdminArea.ViewModels
{
    public class DetailsDeleteCustomerPhotoViewModel: AdminAreaBasePhotoViewModel
    {
        [Display(ResourceType = typeof(Photo), Name = nameof(Customer))]
        public string Customer { get; set; } = default!;
    }
}
