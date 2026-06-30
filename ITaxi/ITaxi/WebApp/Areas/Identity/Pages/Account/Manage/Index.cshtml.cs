// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;

using Base.Resources;
using Google.Apis.PeopleService.v1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Gender = App.Enum.Enum.Gender;
using Index = App.Resources.Areas.Identity.Pages.Account.Manage.Index;
using Photo = App.Domain.Photo;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Index model for account management
/// </summary>
public class IndexModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IWebHostEnvironment _webHostEnvironment;

    /// <summary>
    /// Index model account management constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="context">DB context</param>
    /// <param name="webHostEnvironment">Web host environment</param>
    public IndexModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IWebHostEnvironment webHostEnvironment,
        AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
        _webHostEnvironment = webHostEnvironment;
        _context = context;
    }

    /// <summary>
    /// Users name 
    /// </summary>

    [Display(ResourceType = typeof(Index),
        Name = "UserName")]
    public string Username { get; set; } = default!;

    /// <summary>
    /// Status message
    /// </summary>
    [TempData]
    public string? StatusMessage { get; set; }

    /// <summary>
    /// Input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;
    /// <summary>
    /// List of countries
    /// </summary>
    public SelectList? Countries { get; set; }

    /// <summary>
    /// List of cities
    /// </summary>
    public SelectList? Cities { get; set; }

    /// <summary>
    /// List of changed driver license categories
    /// </summary>
    public ICollection<Guid>? ChangedDriverLicenseCategoriesList { get; set; }
    
    /// <summary>
    /// List of selected driver license categories
    /// </summary>
    public SelectList? SelectedDriverLicenseCategories { get; set; }
    
    /// <summary>
    /// List of driver license categories
    /// </summary>
    public SelectList? DriverLicenseCategories { get; set; }
    
    /// <summary>
    /// List of disability types
    /// </summary>
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
        var countryId = user.CountryId;

        Countries = new SelectList(_context.Countries.
            Include(c => c.CountryName)
            .ThenInclude(t => t.Translations)
          .Select(c => new { c.Id, c.CountryName })
           .ToListAsync().Result.OrderBy(c => (string)c.CountryName), nameof(Country.Id), nameof(Country.CountryName));


        Cities = new SelectList(await _context.Cities
            .OrderBy(c => c.CityName)
            .Select(c => new { c.Id, c.CityName })
            .ToListAsync(), nameof(City.Id),
            nameof(City.CityName));

        DisabilityTypes = new SelectList(_context.DisabilityTypes
            .Include(t => t.DisabilityTypeName)
            .ThenInclude(t => t.Translations)
            .OrderBy(c => c.DisabilityTypeName)
            .Select(c => new { c.Id, c.DisabilityTypeName })
            .ToListAsync().Result.OrderBy(d => (string) d.DisabilityTypeName ), nameof(DisabilityType.Id),
        nameof(DisabilityType.DisabilityTypeName));

        Username = userName!;

        if (User.IsInRole(nameof(Admin)) || User.IsInRole(nameof(Driver)))
            if (admin != null)
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber!,
                    FirstName = firstname,
                    LastName = lastName,
                    Gender = Enum.Parse<Gender>(gender.Value.ToString()),
                    DateOfBirth = dateOfBirth,
                    PersonalIdentifier = admin.PersonalIdentifier,
                    CountryId = countryId,
                    CityId = admin.CityId,
                    AddressOfResidence = admin.Address,
                    ImageFile = user.ProfileImage,
                    
                };

            else if (driver != null)
            {
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
                    Gender = Enum.Parse<Gender>(gender.Value.ToString()),
                    DateOfBirth = dateOfBirth,
                    PersonalIdentifier = driver.PersonalIdentifier,
                    CountryId = countryId,
                    CityId = driver.CityId,
                    AddressOfResidence = driver.Address,
                    DriverLicenseNumber = driver.DriverLicenseNumber,
                    DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate?.Date,
                    ServiceProviderCardIdentifier = driver.ServiceProviderCardIdentifier,
                    ImageFile = user.ProfileImage
                };
            }

        if (User.IsInRole(nameof(Customer)))
        {
            if (customer != null)
            {
                if (customer.DisabilityTypeId == null)
                {
                    Input = new InputModel
                    {
                        PhoneNumber = phoneNumber!,
                        FirstName = firstname,
                        LastName = lastName,
                        Gender = Enum.Parse<Gender>(gender.Value.ToString()),
                        DateOfBirth = dateOfBirth,
                        ImageFile = user.ProfileImage
                    };
                }
                else
                {
                    Input = new InputModel
                    {
                        PhoneNumber = phoneNumber!,
                        FirstName = firstname,
                        LastName = lastName,
                        Gender = Enum.Parse<Gender>(gender.Value.ToString()),
                        CountryId = countryId,
                        DateOfBirth = dateOfBirth,
                        DisabilityId = customer.DisabilityTypeId.Value,
                        ImageFile = user.ProfileImage,
                        
                    };

                    

                }

                

            }
        }
    }

    public async Task GetUserProfilePhotoAsync()
    {
         
        var user = await _userManager.GetUserAsync(User);
        var userProfilePhoto = await _context.Photos.SingleOrDefaultAsync(upp => upp.AppUserId == user!.Id);
        if (Input == null)
        {
            Input = new InputModel(); 
        }
        if (userProfilePhoto == null)
        {
            Input.PhotoPath = "/Images/Icons/icons8-selfies-50.png";
        }
        else
        {
            Input.PhotoPath = userProfilePhoto.PhotoURL;
        }

    }

    /// <summary>
    /// On get async method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        Input = new InputModel();
        await GetUserProfilePhotoAsync();
        await LoadAsync(user);
        await GetUserProfilePhotoAsync();
        return Page();
    }

    /// <summary>
    /// On post async method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        var directoryId = string.Empty;
        var fileNameOnDisk = string.Empty;
        var path = string.Empty;
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        
        if (Input.ImageFile != null)
        {
            var result= await _context.Photos.FirstOrDefaultAsync(au => au.AppUserId == user.Id);
            if (result?.DirectoryTitleId == null)
            {
                directoryId = Guid.NewGuid().ToString();
                string[] directoryNames = { "ProfileImages" };
                path = Path.Combine(_webHostEnvironment.WebRootPath, "Images\\");
                foreach (var name in directoryNames)
                {
                    path += Path.Combine(name + "\\");
                }
                path = Path.Combine(path, directoryId);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);

                }
                
                fileNameOnDisk = "ProfileImage";
                string fileExtension = Path.GetExtension(Input.ImageFile.FileName!);
                if (fileExtension != ".png" && fileExtension != ".jpg")
                {
                    return Content("The file extension isn't acceptable. Acceptable file extensions" +
                        " are .png and .jpg.");
                }
                else
                {
                   fileNameOnDisk += fileExtension;
                   path = Path.Combine(path, fileNameOnDisk);
                   await using var stream = new FileStream(path, FileMode.Create);
                   await Input.ImageFile.CopyToAsync(stream);
                }
            }
        }
        
        var admin = await _context.Admins.SingleOrDefaultAsync(a => a.AppUserId.Equals(user.Id));
        var driver = await _context.Drivers.SingleOrDefaultAsync(d => d.AppUserId.Equals(user.Id));
        var customer = await _context.Customers.SingleOrDefaultAsync(c => c.AppUserId.Equals(user.Id));
        var photoTitle = admin != null ? "AdminProfilePhoto" : driver != null ? "DriverProfilePhoto" :
            customer != null ? "CustomerProfilePhoto" : "Unknown";
        if (string.IsNullOrWhiteSpace(path))
        {
            path = "/Images/Icons/icons8-selfies-50.png";
        }
        var photoUrlPath = Path.GetRelativePath(_webHostEnvironment.WebRootPath, path);
       
        photoUrlPath = "\\" + photoUrlPath;
        Guid? adminId = null;
        Guid? driverId = null;
        Guid? customerId = null;
        var fileName = Input.ImageFile?.FileName;
        if (string.IsNullOrWhiteSpace(fileName))
        {
            fileName = "icons8-selfies-50.png";
        }
        if (admin != null)
        {
            adminId = admin.Id;
        }
        else if (driver != null)
        {
            driverId = driver.Id;
        }
        else
        {
            customerId = customer!.Id;
        }
        if (Input.ImageFile != null)
        {
            var photo = new Photo
            {
                Id = Guid.NewGuid(),
                Title = photoTitle,
                PhotoURL = photoUrlPath,
                FileName = fileName,
                ContentType = Input.ImageFile!.ContentType,
                PhotoFullPath = path,
                DirectoryTitleId = directoryId,
                FileNameInDirectory = fileNameOnDisk,
                AdminId = adminId,
                DriverId = driverId,
                CustomerId = customerId,
                AppUserId = user.Id,
                CreatedBy = User.Identity!.Name,
                CreatedAt = DateTime.UtcNow,
                UpdatedBy = User.Identity!.Name,
                UpdatedAt = DateTime.UtcNow,
            };
            _context.Photos.Add(photo);
            await _context.SaveChangesAsync();
            
        }
        

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

        if (Input.Gender != user.Gender!.Value) user.Gender = Input.Gender;
        if (Input.CountryId.HasValue)
        {
            if (Input.CountryId.Value != user.CountryId)
            {
                user.CountryId = Input.CountryId.Value;
            }
        }
        _context.Users.Update(user);
        await _context.SaveChangesAsync();

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

            if (Input.DriverLicenseExpiryDate?.Date != driver.DriverLicenseExpiryDate?.Date)
            {
                driver.DriverLicenseExpiryDate = Input.DriverLicenseExpiryDate!.Value.Date;
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
        //var photo = new Photo() { } 
        await _signInManager.RefreshSignInAsync(user);

        StatusMessage = Base.Resources.Identity.Pages.Account.Manage.Index.ProfileUpdated;

        return RedirectToPage();
    }
/*
    /// <summary>
    /// Setting a profile image for an user
    /// </summary>
    /// <returns>IActionResult</returns>
    private async Task<IActionResult> SavingImage()
    {
        
        
       
        

        await _context.SaveChangesAsync();
        return RedirectToPage();
    }*/

    /// <summary>
    ///  Input model
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Phone number
        /// </summary>
        [Phone]
        [Display(ResourceType = typeof(Index), Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;

        /// <summary>
        /// First name
        /// </summary>
        [StringLength(50, MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Index),
            Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// Last name
        /// </summary>
        [StringLength(50, MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(Index),
            Name = nameof(LastName))]
        public string LastName { get; set; } = default!;
        
        /// <summary>
        /// Gender
        /// </summary>
        [EnumDataType(typeof(Gender))]
        [Display(ResourceType = typeof(Index),
            Name = nameof(Gender))]
        public Gender Gender { get; set; }

        /// <summary>
        /// Date of birth
        /// </summary>
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Index), Name = "DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Personal identifier
        /// </summary>
        [StringLength(50, MinimumLength = 1)]
        [Display(ResourceType = typeof(Index), Name = "PersonalIdentifier")]
        public string? PersonalIdentifier { get; set; }
        /// <summary>
        /// Country id for user's profile page 
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "Country")]
        public Guid? CountryId { get; set; }

        /// <summary>
        /// City id for user's profile page
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "City")]
        public Guid? CityId { get; set; }

        /// <summary>
        /// Address of residence
        /// </summary>
        [StringLength(50, MinimumLength = 1)]
        [Display(ResourceType = typeof(Index), Name = "AddressOfResidence")]
        public string? AddressOfResidence { get; set; } 
        
        /// <summary>
        /// Disability id for user's profile page
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "DisabilityType")]
        public Guid? DisabilityId { get; set; }
        
        /// <summary>
        /// Driver license number
        /// </summary>
        [MaxLength(15, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [MinLength(2, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMinLength")]
        [StringLength(15, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Index), Name = "DriverLicenseNumber")]
        public string? DriverLicenseNumber { get; set; }

        /// <summary>
        /// List of selected driver license categories for the user's profile page
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "SelectedDriverLicenseCategories")]
        public SelectList? SelectedDriverLicenseCategories { get; set; }

        /// <summary>
        /// Driver license categories
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "DriverLicenseCategories")]
        public SelectList? DriverLicenseCategories { get; set; }

        /// <summary>
        /// List of changed driver license categories for the user's profile page
        /// </summary>
        public List<Guid>? ChangedDriverLicenseCategoriesList  { get; set; }
        
        /// <summary>
        /// Driver license expiry date
        /// </summary>
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(Index), Name = "DriverLicenseExpiryDate")]
        public DateTime? DriverLicenseExpiryDate { get; set; }
        
        /// <summary>
        /// Image file
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "ProfileImage")]
        public IFormFile? ImageFile { get; set; }

        /// <summary>
        /// The title for the default image photo
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "ProfileImage")]
        public string? DefaultPhotoTitle { get; set; } = "icons8-selfies-50.png";
       
        /// <summary>
        /// The path to a user photo
        /// </summary>
        public string? PhotoPath { get; set; }
        /// <summary>
        /// The service provider card identifier
        /// </summary>
        [Display(ResourceType = typeof(Index), Name = "ServiceProviderCardIdentifier")]
        public string? ServiceProviderCardIdentifier { get; set; } 
    }
}