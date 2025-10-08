using App.BLL;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Helpers;

public async Task<IActionResult> ChooseView(Guid id)
{

    var roleName = User.GettingUserRoleName();
    var vehicle = await AppBLL.Vehicles.GettingVehicleWithIncludesByIdAsync(id, roleName: roleName);
    if (vehicle == null)
        return NotFound();

    int numberOfPhotos = await _appBLL.Photos.GetPhotoCountByVehicleIdAsync(vehicle.Id, roleName: roleName);
    if (numberOfPhotos == 0)
    {
        return RedirectToAction("VehicleImagesUpload", "Photos", new { id = vehicle.Id });
    }
    else
    {
        var vm = new VehicleGalleryAdminViewModel()
        {
            Photos = await _appBLL.Photos.GetAllPhotosByVehicleIdWithIncludesAsync(vehicle.Id)
        };
        return View("Gallery", vm);
    }


}