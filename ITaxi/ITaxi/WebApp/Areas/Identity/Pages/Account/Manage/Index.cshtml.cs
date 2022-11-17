// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel.DataAnnotations;
using App.DAL.EF;
using App.Domain;
using App.Domain.Enum;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
                    PersonalIdentifier = admin.PersonalIdentifier!,
                    ImageFile = user.ProfileImage
                };
            #warning Ask if there is a better way to implement it
            else if (driver != null)
            {
                Input = new InputModel
                {
                    PhoneNumber = phoneNumber!,
                    FirstName = firstname,
                    LastName = lastName,
                    Gender = gender,
                    DateOfBirth = dateOfBirth,
                    PersonalIdentifier = driver.PersonalIdentifier!,
                    ImageFile = user.ProfileImage
                };
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

            _context.Admins.Update(admin);
            await _context.SaveChangesAsync();
        }

        if (driver != null)
        {
            if (Input.PersonalIdentifier != driver.PersonalIdentifier)
            {
                driver.PersonalIdentifier = Input.PersonalIdentifier!;
            }

            _context.Drivers.Update(driver);
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

        [Display(ResourceType = typeof(Index), Name = "PersonalIdentifier")]
        public string? PersonalIdentifier { get; set; }

        [Display(ResourceType = typeof(Index), Name = "ProfileImage")]
        public IFormFile? ImageFile { get; set; }

        [Display(ResourceType = typeof(Index), Name = "ProfileImage")]
        public string PhotoPath { get; set; } = "icons8-selfies-50.png";
    }
}