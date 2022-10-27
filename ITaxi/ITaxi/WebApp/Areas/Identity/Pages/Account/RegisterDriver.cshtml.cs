// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using App.DAL.EF;
using App.Domain;
using App.Domain.Enum;
using App.Domain.Identity;
using App.Resources.Areas.Identity.Pages.Account;
using Base.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account;

public class RegisterDriverModel : PageModel
{
    private readonly AppDbContext _context;
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore<AppUser> _emailStore;
    private readonly ILogger<RegisterDriverModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserStore<AppUser> _userStore;


    public RegisterDriverModel(
        UserManager<AppUser> userManager,
        IUserStore<AppUser> userStore,
        SignInManager<AppUser> signInManager,
        ILogger<RegisterDriverModel> logger,
        IEmailSender emailSender, AppDbContext context
    )
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
        _context = context;
        Cities = new SelectList(_context.Cities
                .OrderBy(c => c.CityName)
                .Select(c => new {c.Id, c.CityName}).ToList(),
            nameof(City.Id), nameof(City.CityName));
        DriverLicenseCategories = new SelectList(_context.DriverLicenseCategories
                .OrderBy(d => d.DriverLicenseCategoryName)
                .Select(d => new {d.Id, d.DriverLicenseCategoryName}).ToList(),
            nameof(DriverLicenseCategory.Id), nameof(DriverLicenseCategory.DriverLicenseCategoryName));
    }


    public SelectList DriverLicenseCategories { get; set; }

    public SelectList? Cities { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }


    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }

    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
#warning driver's dateOfBirth needs a custom validation rule
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                PhoneNumber = Input.PhoneNumber,
                Email = Input.Email,
                EmailConfirmed = true
            };

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);

#warning ask if this is the right way to add a claim in my app context
            result = await _userManager.AddClaimAsync(user, new Claim("aspnet.firstname", user.FirstName));
            result = await _userManager.AddClaimAsync(user, new Claim("aspnet.lastname", user.LastName));


            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    null,
                    new {area = "Identity", userId, code, returnUrl},
                    Request.Scheme);

                await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");
                await _userManager.AddToRoleAsync(user, "Driver");
                await _context.SaveChangesAsync();
                var driver = new Driver
                {
#warning driver's driver license expiry date needs a custom validation rule
                    AppUserId = user.Id, PersonalIdentifier = Input.PersonalIdentifier,
                    Address = Input.Address, CityId = Input.CityId,
                    DriverLicenseNumber = Input.DriverLicenseNumber,
                    DriverLicenseExpiryDate = Input.ExpiryDate
                };
                await _context.Drivers.AddAsync(driver);
                if (Input.DriverAndDriverLicenseCategories != null)
                {
#warning Adding driver and driver license categories needs a custom validation rule

                    foreach (var driverLicenseCategoryId in Input.DriverAndDriverLicenseCategories)
                    {
                        var driverAndDriverLicenseCategories = new DriverAndDriverLicenseCategory
                        {
                            DriverId = driver.Id,
                            DriverLicenseCategoryId = driverLicenseCategoryId
                        };
                        await _context.DriverAndDriverLicenseCategories.AddAsync(driverAndDriverLicenseCategories);
                        await _context.SaveChangesAsync();
                    }

#warning temporarly solution
                    result = await _userManager.AddToRoleAsync(user, nameof(Driver));
                    if (!result.Succeeded) throw new ApplicationException("Cannot add an user to a role");
                }

                if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    return RedirectToPage("RegisterConfirmation", new {email = Input.Email, returnUrl});

                await _signInManager.SignInAsync(user, false);
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    private AppUser CreateUser()
    {
        try
        {
            return Activator.CreateInstance<AppUser>();
        }
        catch
        {
            throw new InvalidOperationException($"Can't create an instance of '{nameof(AppUser)}'. " +
                                                $"Ensure that '{nameof(AppUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                                                "override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
        }
    }

    private IUserEmailStore<AppUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException("The default UI requires a user store with email support.");
        return (IUserEmailStore<AppUser>) _userStore;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(DriverRegister), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(DriverRegister), Name = nameof(LastName))]
        public string LastName { get; set; } = default!;

        [Display(ResourceType = typeof(DriverRegister), Name = nameof(Gender))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(DriverRegister), Name = nameof(DateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        [StringLength(50)]
        [Display(ResourceType = typeof(DriverRegister), Name = nameof(PersonalIdentifier))]
        public string? PersonalIdentifier { get; set; }

        [DataType(DataType.Text)]
        [Display(ResourceType = typeof(DriverRegister), Name = "City")]
        public Guid CityId { get; set; }

        [Display(ResourceType = typeof(DriverRegister), Name = "DriverLicenseCategories")]
        public ICollection<Guid>? DriverAndDriverLicenseCategories { get; set; }

        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Text)]
        [StringLength(25)]
        [Display(ResourceType = typeof(DriverRegister), Name = nameof(DriverLicenseNumber))]
        public string DriverLicenseNumber { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(DriverRegister), Name = "DriverLicensesExpiryDate")]
        public DateTime ExpiryDate { get; set; }


        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Text)]
        [StringLength(72, MinimumLength = 2, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(AdminRegister), Name = "AddressOfResidence")]
        public string Address { get; set; } = default!;

        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageStringLengthMax")]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(Common), Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageEmail")]
        [Display(ResourceType = typeof(DriverRegister), Name = "Email")]
        public string Email { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [StringLength(100, ErrorMessageResourceType = typeof(Common)
            , ErrorMessageResourceName = "StringLengthAttributeErrorMessage",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Common), Name = nameof(Password))]
        public string Password { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Common), Name = nameof(ConfirmPassword))]
        [Compare(nameof(Password), ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageComparePasswords")]
        public string ConfirmPassword { get; set; } = default!;
    }
}