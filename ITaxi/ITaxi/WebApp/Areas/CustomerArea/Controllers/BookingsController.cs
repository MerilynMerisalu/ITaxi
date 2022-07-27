using App.Contracts.DAL;
using App.Domain;
using App.Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.CustomerArea.ViewModels;

namespace WebApp.Areas.CustomerArea.Controllers;

[Area(nameof(CustomerArea))]
[Authorize(Roles = "Admin, Customer")]
public class BookingsController : Controller
{
    private readonly IAppUnitOfWork _uow;

    public BookingsController(IAppUnitOfWork uow)
    {
        _uow = uow;
    }


    // GET: CustomerArea/Bookings
    public async Task<IActionResult> Index()
    {
        return View(await _uow.Bookings.GettingAllOrderedBookingsAsync());
    }

    // GET: CustomerArea/Bookings/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();

        var booking = await _uow.Bookings.FirstOrDefaultAsync(id.Value);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;

        vm.City = booking.City!.CityName;
        vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");

        return View(vm);
    }

    // GET: CustomerArea/Bookings/Create
    public async Task<IActionResult> Create()
    {
        var vm = new CreateBookingViewModel();
        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesAsync(),
            nameof(City.Id), nameof(City.CityName));
        vm.VehicleTypes = new SelectList(await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName));

        return View(vm);
    }

    // POST: CustomerArea/Bookings/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBookingViewModel vm)
    {
        var booking = new Booking();
        if (ModelState.IsValid)
        {
            booking.Id = Guid.NewGuid();
            booking.CityId = vm.CityId;
            booking.Customer = await _uow.Customers.FirstAsync();
            booking.Driver = await _uow.Drivers.FirstAsync();
#warning needs fixing
            booking.Schedule = await _uow.Schedules.FirstAsync();
#warning needs fixing
            booking.Vehicle =
                await _uow.Vehicles.FirstAsync();
            booking.AdditionalInfo = vm.AdditionalInfo;
            booking.DestinationAddress = vm.DestinationAddress;
            booking.PickupAddress = vm.PickupAddress;
            booking.VehicleTypeId = vm.VehicleTypeId;
            booking.HasAnAssistant = vm.HasAnAssistant;
            booking.NumberOfPassengers = vm.NumberOfPassengers;
            booking.StatusOfBooking = StatusOfBooking.Awaiting;
#warning Booking PickUpDateAndTime needs a custom validation
            booking.PickUpDateAndTime = DateTime.Parse(vm.PickUpDateAndTime).ToUniversalTime();
            _uow.Bookings.Add(booking);

            var drive = new Drive
            {
                Id = new Guid(),
                Booking = booking,
                DriverId = booking.DriverId
            };

            _uow.Drives.Add(drive);
            await _uow.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        vm.Cities = new SelectList(await _uow.Cities.GetAllOrderedCitiesWithoutCountyAsync(), nameof(City.Id),
            nameof(City.CityName), nameof(booking.CityId));
        vm.VehicleTypes = new SelectList(await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id), nameof(VehicleType.VehicleTypeName),
            nameof(booking.VehicleTypeId));

        return View(vm);
    }

    // GET: AdminArea/Bookings/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        var vm = new EditBookingViewModel();
        if (id == null) return NotFound();

        var booking = await _uow.Bookings.SingleOrDefaultAsync(b => b!.Id.Equals(id));
        if (booking == null) return NotFound();


        vm.Cities = new SelectList(await _uow.Cities.GetAllCitiesWithoutCountyAsync()
            , nameof(City.Id), nameof(City.CityName));
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.CityId = booking.CityId;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleTypes = new SelectList(
            await _uow.VehicleTypes.GetAllVehicleTypesOrderedAsync(),
            nameof(VehicleType.Id)
            , nameof(VehicleType.VehicleTypeName));
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.VehicleTypeId = booking.VehicleTypeId;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime;
        return View(vm);
    }

    // POST: AdminArea/Bookings/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, EditBookingViewModel vm)
    {
        var booking = await _uow.Bookings.SingleOrDefaultAsync(b => b!.Id.Equals(id));
        if (booking != null && id != booking.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                if (booking != null)
                {
                    booking.Id = id;
                    booking.CityId = vm.CityId;
                    booking.Customer = await _uow.Customers.FirstAsync();
                    booking.Driver = await _uow.Drivers.FirstAsync();
#warning needs fixing
                    booking.Schedule = await _uow.Schedules.FirstAsync();
#warning needs fixing
                    booking.Vehicle = await _uow.Vehicles.FirstAsync();
                    booking.AdditionalInfo = vm.AdditionalInfo;
                    booking.DestinationAddress = vm.DestinationAddress;
                    booking.PickupAddress = vm.PickupAddress;
                    booking.VehicleTypeId = vm.VehicleTypeId;
                    booking.HasAnAssistant = vm.HasAnAssistant;
                    booking.NumberOfPassengers = vm.NumberOfPassengers;
                    booking.StatusOfBooking = StatusOfBooking.Awaiting;
                    booking.PickUpDateAndTime = vm.PickUpDateAndTime;


                    _uow.Bookings.Update(booking);
                }

                var drive = await _uow.Drives
                    .SingleOrDefaultAsync(d => d!.Booking!.Id.Equals(booking!.Id), false);
                if (drive != null)
                {
                    if (booking != null)
                    {
                        drive.DriverId = booking.DriverId;
                        drive.Booking = booking;
                    }

                    _uow.Drives.Update(drive);
                    await _uow.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (booking != null && !BookingExists(booking.Id))
                    return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        return View(vm);
    }

    // GET: CustomerArea/Bookings/Decline/5
    public async Task<IActionResult> Decline(Guid? id)
    {
        var vm = new DeclineBookingViewModel();
        if (id == null) return NotFound();

        var booking = await _uow.Bookings.FirstOrDefaultAsync(id.Value);
        if (booking == null) return NotFound();

        vm.Id = booking.Id;
        vm.City = booking.City!.CityName;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToLocalTime().ToString("g");


        return View(vm);
    }

    // POST: AdminArea/Bookings/Decline/5
    [HttpPost]
    [ActionName(nameof(Decline))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeclineConfirmed(Guid id)
    {
        var booking = await _uow.Bookings.FirstOrDefaultAsync(id);
        if (booking != null)
        {
            var drive = await _uow.Drives.SingleOrDefaultAsync(d => d!.Booking!.Id.Equals(id));
            _uow.Bookings.BookingDecline(booking);
            drive!.Booking = booking;
            _uow.Bookings.Update(booking);
            await _uow.SaveChangesAsync();
            _uow.Drives.Update(drive);
        }

        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }


    // GET: CustomerArea/Bookings/Delete/5
    public async Task<IActionResult> Delete(Guid? id)
    {
        var vm = new DetailsDeleteBookingViewModel();
        if (id == null) return NotFound();

        var booking = await _uow.Bookings.FirstOrDefaultAsync(id.Value, false);
        if (booking == null) return NotFound();
        vm.Id = booking.Id;
        vm.City = booking.City!.CityName;
        vm.Vehicle = booking.Vehicle!.VehicleIdentifier;
        vm.AdditionalInfo = booking.AdditionalInfo;
        vm.DestinationAddress = booking.DestinationAddress;
        vm.PickupAddress = booking.PickupAddress;
        vm.VehicleType = booking.VehicleType!.VehicleTypeName;
        vm.HasAnAssistant = booking.HasAnAssistant;
        vm.NumberOfPassengers = booking.NumberOfPassengers;
        vm.StatusOfBooking = booking.StatusOfBooking;
        vm.PickUpDateAndTime = booking.PickUpDateAndTime.ToString("g");
        return View(vm);
    }


    // POST: CustomerArea/Bookings/Delete/5
    [HttpPost]
    [ActionName(nameof(Delete))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        var booking = await _uow.Bookings.SingleOrDefaultAsync(b => b != null && b.Id.Equals(id), false);
        var drive = await _uow.Drives.SingleOrDefaultAsync(d => d != null && d.Booking!.Id.Equals(id), false);
        var comment =
            await _uow.Comments.SingleOrDefaultAsync(c => drive != null && c != null && c.DriveId.Equals(drive.Id),
                false);
        if (comment != null) _uow.Comments.Remove(comment);
        if (drive != null) _uow.Drives.Remove(drive);
        if (booking != null) _uow.Bookings.Remove(booking);
        await _uow.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool BookingExists(Guid id)
    {
        return _uow.Bookings.Exists(id);
    }

    /// <summary>
    ///     Search records by city name
    /// </summary>
    /// <param name="search">City name</param>
    /// <returns>An index view with search results</returns>
    [HttpPost]
    public async Task<IActionResult> SearchByCityAsync([FromForm] string search)
    {
        var results = await _uow.Bookings.SearchByCityAsync(search);
        return View(nameof(Index), results);
    }
}