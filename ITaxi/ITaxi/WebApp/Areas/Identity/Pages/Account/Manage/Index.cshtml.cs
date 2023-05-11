// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel.DataAnnotations;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using App.Enum.Enum;
using Base.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Index = App.Resources.Areas.Identity.Pages.Account.Manage.Index;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public IndexModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager, AppDbContext context,
        IWebHostEnvironment webHostEnvironment)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>

    [Display(ResourceType = typeof(Index),
        Name = "UserName")]
    public string Username { get; set; } = default!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [TempData]
    public string? StatusMessage { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;
    public SelectList? Cities { get; set; }

    public ICollection<Guid>? ChangedDriverLicenseCategoriesList { get; set; }
    public SelectList? SelectedDriverLicenseCategories { get; set; }
    public SelectList? DriverLicenseCategories { get; set; }
    public SelectList? DisabilityTypes { get; set; }
    private async Task LoadAsync(AppUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        var admin = await _context.Admins.SingleOrDefaultAsync(a => a.AppUserId.Equals(user.Id));
        var driver = await _context.Drivers.SingleOrDefaultAsync(d => d.AppUserId.Equals(user.Id));
        var customer = await _context.Customers.SingleOrDefaultAsync(c => c.AppUserId.Equals(user.Id));

        var photoPath = _context.Photos.Where(x => x.AppUserId == user.Id)
            .Select(x => x.PhotoURL)
            .FirstOrDefault();
        var firstname = user.FirstName;
        var lastName = user.LastName;
        var gender = user.Gender;
        var dateOfBirth = user.DateOfBirth.Date;
        
        Cities = new SelectList(await _context.Cities
            .OrderBy(c => c.CityName)
            .Select(c => new {c.Id, c.CityName})
            .ToListAsync(), nameof(City.Id), 
            nameof(City.CityName));
        

            DisabilityTypes = new SelectList(await _context.DisabilityTypes
                .Include(t => t.DisabilityTypeName)
                .ThenInclude(t => t.Translations)
                .OrderBy(c => c.DisabilityTypeName)
                .Select(c => new {c.Id, c.DisabilityTypeName})
                .ToListAsync(), nameof(DisabilityType.Id),
            nameof(DisabilityType.DisabilityTypeName));
        
            

        Username = userName!;

        if (User.IsInRole(nameof(Admin)) || User.IsInRole(nameof(Driver)))
            if (admin != null )
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber!,
                    FirstName = firstname,
                    LastName = lastName,
                    Gender = gender,
                    DateOfBirth = dateOfBirth,
                    PersonalIdentifier = admin.PersonalIdentifier,
                    CityId = admin.CityId,
                    AddressOfResidence = admin.Address,
                    ImageFile = user.ProfileImage
                };
            #warning Ask if there is a better way to implement it
            else if (driver != null)
            {
#warning add the ability to change the list of driver license categories with js
                SelectedDriverLicenseCategories = new SelectList(await
                        _context.DriverAndDriverLicenseCategories
                            .Include(d => d.Driver)
                            .Include(dlc => dlc.DriverLicenseCategory)
                            .OrderBy(dlc => dlc.DriverLicenseCategory!.DriverLicenseCategoryName)
                            .Where(dlc => dlc.DriverId.Equals(driver!.Id))
                            .Select(dlc => new DriverLicenseCategory
                            {
                                Id = dlc.DriverLicenseCategoryId,
                                DriverLicenseCategoryName = dlc.DriverLicenseCategory!.DriverLicenseCategoryName
                            }).ToListAsync(), nameof(DriverLicenseCategory.Id), 
                    nameof(DriverLicenseCategory.DriverLicenseCategoryName));
                DriverLicenseCategories = new SelectList(await _context.DriverLicenseCategories
                        .Include(d => d.Drivers)
                        .Where(dlc => !dlc.Drivers!.Any(d => d.DriverId.Equals(driver.Id)))
                        .OrderBy(dlc => dlc.DriverLicenseCategoryName).ToListAsync(),
                    nameof(DriverLicenseCategory.Id),
                    nameof(DriverLicenseCategory.DriverLicenseCategoryName));
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber!,
                    FirstName = firstname,
                    LastName = lastName,
                    Gender = gender,
                    DateOfBirth = dateOfBirth,
                    PersonalIdentifier = driver.PersonalIdentifier,
                    CityId = driver.CityId,
                    AddressOfResidence = driver.Address,
                    DriverLicenseNumber = driver.DriverLicenseNumber,
                    DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate.Date,
                    
                    ImageFile = user.ProfileImage
                };
            }

        if (User.IsInRole(nameof(Customer)))
        {
            if (customer != null)
            {
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber!,
                    FirstName = firstname,
                    LastName = lastName,
                    Gender = gender,
                    DateOfBirth = dateOfBirth,
                    DisabilityId = customer.DisabilityTypeId,
                    ImageFile = user.ProfileImage
                };
            }
        }
        
        if (user.ProfilePhoto != null)
            Input.PhotoPath = $"data:image/*;base64,{Convert.ToBase64String(user.ProfilePhoto!)}";
        else
            Input.PhotoPath = Path.Combine(_webHostEnvironment.WebRootPath + "/Images/icons8-selfies-50.png");
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");


        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        if (Input.ImageFile != null) await SavingImage();
        
        var admin = await _context.Admins.SingleOrDefaultAsync(a => a.AppUserId.Equals(user.Id));
        var driver = await _context.Drivers.SingleOrDefaultAsync(d => d.AppUserId.Equals(user.Id));
        var customer = await _context.Customers.SingleOrDefaultAsync(c => c.AppUserId.Equals(user.Id));

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        if (Input.FirstName != user.FirstName) user.FirstName = Input.FirstName;

        if (Input.LastName != user.LastName) user.LastName = Input.LastName;

        if (Input.Gender != user.Gender) user.Gender = Input.Gender;
        if (admin != null)
        {
            if (Input.PersonalIdentifier != admin.PersonalIdentifier)
            {
                admin.PersonalIdentifier = Input.PersonalIdentifier!;
            } 
            if (Input.CityId != admin.CityId)
            {
                if (Input.CityId != null) admin.CityId = Input.CityId.Value;
            }

            if (Input.AddressOfResidence != admin.Address)
            {
                if (Input.AddressOfResidence != null) admin.Address = Input.AddressOfResidence;
            }

            admin.UpdatedBy = User.Identity!.Name;
            admin.UpdatedAt = DateTime.Now.ToUniversalTime();

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        if (driver != null)
        {
            if (Input.PersonalIdentifier != driver.PersonalIdentifier)
            {
                driver.PersonalIdentifier = Input.PersonalIdentifier!;
            }

            if (Input.CityId != driver.CityId)
            {
                if (Input.CityId != null) driver.CityId = Input.CityId.Value;
            }

            if (Input.AddressOfResidence != driver.Address)
            {
                if (Input.AddressOfResidence != null) driver.Address = Input.AddressOfResidence;
            }

            if (Input.DriverLicenseNumber != driver.DriverLicenseNumber)
            {
                if (Input.DriverLicenseNumber != null) driver.DriverLicenseNumber = Input.DriverLicenseNumber;
            }

            if (Input.DriverLicenseExpiryDate.Date != driver.DriverLicenseExpiryDate.Date)
            {
                driver.DriverLicenseExpiryDate = Input.DriverLicenseExpiryDate;
            }
            
            

            if (Input.ChangedDriverLicenseCategoriesList != null)
            {
                foreach (var driverAndDriverLicenseCategory in Input.ChangedDriverLicenseCategoriesList!)
                {
                    var driverAndDriverLicenseCategoryEntity = new DriverAndDriverLicenseCategory()
                    {
                        Id = new Guid(),
                        DriverId = driver.Id,
                        DriverLicenseCategoryId = driverAndDriverLicenseCategory
                    };
                    await _context.DriverAndDriverLicenseCategories.AddAsync(driverAndDriverLicenseCategoryEntity);
                }

                await _context.SaveChangesAsync();
            }
            
            driver.UpdatedBy = User.Identity!.Name;
            driver.UpdatedAt = DateTime.Now.ToUniversalTime();


            _context.Drivers.Update(driver);
            await _context.SaveChangesAsync();

        }

        if (customer != null )
        {
            if (customer.DisabilityTypeId != Input.DisabilityId)
            {
                customer.DisabilityTypeId = Input.DisabilityId!.Value;
            }
            customer.UpdatedBy = User.Identity!.Name;
            customer.UpdatedAt = DateTime.Now.ToUniversalTime();
                
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
        
        await _context.SaveChangesAsync();
        _context.Users.Update(user);

        await _signInManager.RefreshSignInAsync(user);

        StatusMessage = Base.Resources.Identity.Pages.Account.Manage.Index.ProfileUpdated;

        return RedirectToPage();
    }

    /// <summary>
    ///     Setting a profile image for an user
    /// </summary>
    /// <returns>IActionResult</returns>
    private async Task<IActionResult> SavingImage()
    {
        var user = await _userManager.GetUserAsync(User);
        if (Input.ImageFile != null)
        {
            using (var memoryStream = new MemoryStream())
            {
                await Input.ImageFile!.CopyToAsync(memoryStream);
                user.ProfilePhoto = memoryStream.ToArray();
                user.ProfilePhotoName = Path.GetFileName(Input.ImageFile.FileName);
            }

            _context.Users.Update(user);
        }

        Input.PhotoPath = $"data:image/*;base64,{Convert.ToBase64String(user.ProfilePhoto!)}";

        await _context.SaveChangesAsync();
        return RedirectToPage();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Phone]
        [Display(ResourceType = typeof(Index), Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;

        [StringLength(50, MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Index),
            Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;

        [StringLength(50, MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Index),
            Name = nameof(LastName))]
        public string LastName { get; set; } = default!;


        [EnumDataType(typeof(Gender))]
        [Display(ResourceType = typeof(Index),
            Name = nameof(Gender))]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Index), Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Display(ResourceType = typeof(Index), Name = "PersonalIdentifier")]
        public string? PersonalIdentifier { get; set; }

        [Display(ResourceType = typeof(Index), Name = "City")]
        public Guid? CityId { get; set; }

        [StringLength(50, MinimumLength = 1)]
        [Display(ResourceType = typeof(Index), Name = "AddressOfResidence")]
        public string? AddressOfResidence { get; set; } 
        
        [Display(ResourceType = typeof(Index), Name = "DisabilityType")]
        
        public Guid? DisabilityId { get; set; }
        
        
        [MaxLength(15, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [MinLength(2, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
        [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Index), Name = "DriverLicenseNumber")]
        public string? DriverLicenseNumber { get; set; }

        [Display(ResourceType = typeof(Index), Name = "SelectedDriverLicenseCategories")]
        public SelectList? SelectedDriverLicenseCategories { get; set; }

        [Display(ResourceType = typeof(Index), Name = "DriverLicenseCategories")]
        public SelectList? DriverLicenseCategories { get; set; }

        public List<Guid>? ChangedDriverLicenseCategoriesList  { get; set; }
        
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Index), Name = "DriverLicenseExpiryDate")]
        public DateTime DriverLicenseExpiryDate { get; set; }

        
        [Display(ResourceType = typeof(Index), Name = "ProfileImage")]
        public IFormFile? ImageFile { get; set; }

        [Display(ResourceType = typeof(Index), Name = "ProfileImage")]
        public string PhotoPath { get; set; } = "icons8-selfies-50.png";
    }
}