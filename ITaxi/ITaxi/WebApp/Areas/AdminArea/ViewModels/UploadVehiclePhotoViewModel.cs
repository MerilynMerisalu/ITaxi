namespace WebApp.Areas.AdminArea.ViewModels
{
    public class UploadVehiclePhotoViewModel
    {
        public Guid VehicleImageId { get; set; }
        public IFormFile Image { get; set; }
        public int VehicleImageNumber { get; set; }

    }
}
