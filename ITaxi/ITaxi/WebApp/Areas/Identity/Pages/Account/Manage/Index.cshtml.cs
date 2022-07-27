// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using App.DAL.EF;
using App.Domain.Enum;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
        //var photoPath = _context.Photos.Where(x => x.AppUserId == user.Id)
        //    .Select(x => x.PhotoName)
        //    .FirstOrDefault();
        var firstname = user.FirstName;
        var lastName = user.LastName;
        var gender = user.Gender;
        var dateOfBirth = user.DateOfBirth.Date;


        /*if (User.IsInRole(nameof(Admin)) || User.IsInRole(nameof(Driver)))
        {
            
            if (expr)
            {
                
            }
        }*/

        /*if (User.IsInRole(nameof(Customer)))
        {
            
        }*/


        Username = userName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber,
            FirstName = firstname,
            LastName = lastName,
            Gender = gender,
            DateOfBirth = dateOfBirth,
            ImageFile = user.ProfileImage
        };


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

        await SavingImage();
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

        await _context.SaveChangesAsync();
        _context.Users.Update(user);

        await _signInManager.RefreshSignInAsync(user);

        StatusMessage = "Your profile has been updated";

        return RedirectToPage();
    }

    /// <summary>
    ///     Setting a profile image for an user
    /// </summary>
    /// <returns>IActionResult</returns>
    private async Task<IActionResult> SavingImage()
    {
        var user = await _userManager.GetUserAsync(User);
        using (var memoryStream = new MemoryStream())
        {
            await Input.ImageFile!.CopyToAsync(memoryStream);
            user.ProfilePhoto = memoryStream.ToArray();
            user.ProfilePhotoName = Path.GetFileName(Input.ImageFile.FileName);
        }

        _context.Users.Update(user);


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
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; } = default!;

        [StringLength(50, MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = default!;

        [StringLength(50, MinimumLength = 1)]
        [DataType(DataType.Text)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = default!;


        [EnumDataType(typeof(Gender))]
        [DisplayName(nameof(App.Domain.Enum.Gender))]
        public Gender Gender { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date of Birth")]
        public DateTime DateOfBirth { get; set; }


        public IFormFile? ImageFile { get; set; }

        public string PhotoPath { get; set; } = "icons8-selfies-50.png";
    }
}