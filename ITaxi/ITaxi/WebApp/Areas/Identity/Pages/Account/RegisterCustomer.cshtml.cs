// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.DAL.DTO.AdminArea;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using App.Enum.Enum;
using App.Resources.Areas.Identity.Pages.Account;
using Base.Contracts.Mappers;
using Base.Resources;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using WebApp.ApiControllers.Identity;
using WebApp.Controllers;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Register customer model controller
/// </summary>
public class RegisterCustomerModel : PageModel
{
    
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore<AppUser> _emailStore;
    private readonly ILogger<RegisterCustomerModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserStore<AppUser> _userStore;
    private readonly IAppBLL _appBLL;

    /// <summary>
    /// Register customer model constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="userStore">Store for user's</param>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="logger">Logger for customer register</param>
    /// <param name="emailSender">Email sender</param>
    /// <param name="appBLL">AppBLL</param>

    public RegisterCustomerModel(
        UserManager<AppUser> userManager,
        IUserStore<AppUser> userStore,
        SignInManager<AppUser> signInManager,
        ILogger<RegisterCustomerModel> logger,
        IEmailSender emailSender, 
        
         IAppBLL appBLL)
    {
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _signInManager = signInManager;
        _logger = logger;
        _emailSender = emailSender;
       
        _appBLL = appBLL;
        DisabilityTypes = new SelectList(_appBLL.DisabilityTypes.GetAllOrderedDisabilityTypes(), nameof(DisabilityType.Id), nameof(DisabilityType.DisabilityTypeName));
                
        Countries = new SelectList(_appBLL.Countries.GetAllCountriesOrderedByCountryName(), nameof(Country.Id), nameof(Country.CountryName));
        Counties = new SelectList(Enumerable.Empty<County>(), nameof(County.Id), nameof(County.CountyName));

        Cities = new SelectList(Enumerable.Empty<City>(), nameof(City.Id), nameof(City.CityName));
    }

    /// <summary>
    /// Customer disability types
    /// </summary>
    public SelectList? DisabilityTypes { get; set; }
    /// <summary>
    /// List of countries
    /// </summary>
    public SelectList? Countries { get; set; }
    /// <summary>
    /// List of counties
    /// </summary>
    public SelectList? Counties { get; set; }
    /// <summary>
    /// List of cities
    /// </summary>
    public SelectList? Cities { get; set; }
    /// <summary>
    ///  Input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    ///  Return url
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    ///  External logins
    /// </summary>
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }
    
    /// <summary>
    /// On get async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    public async Task OnGetAsync(string? returnUrl = null)
    {
        ReturnUrl = returnUrl;
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
    }
    public async Task<IActionResult> OnGetSetDropDownCountiesListAsync( Guid countryId)
    {
        // Select the Counties for the currently selected CountryId
        var counties = await _appBLL.Counties.GetAllCountiesOrderedByCountyNameByCountryIdAsync(countryId, showDeleted: false, showIgnored: false);

        var result = counties.Select(c => new
        {
            value = c.Id,
            text = c.CountyName
        });


        return new JsonResult(result);
    }

    public async Task<IActionResult> OnGetSetDropDownCitiesListAsync([FromQuery] Guid countyId)
    {
        // Select the Counties for the currently selected CountryId
        var cities = await _appBLL.Cities.GetCitiesByCountyIdAsync(countyId);
        var result = cities.Select(c => new
        {
            value = c.Id,
            text = c.CityName
        });

        return new JsonResult(result);
    }
    /// <summary>
    /// On post async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Url</returns>
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");
        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        if (ModelState.IsValid)
        {
            var user = new AppUser
            {
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Gender = Input.Gender,
                DateOfBirth = Input.DateOfBirth,
                PhoneNumber = Input.PhoneNumber,
                CountryId = Input.CountryId.Value,
                CountyId = Input.CountyId.Value,
                CityId = Input.CityId.Value,
                Address = Input.Address!,
                Email = Input.Email,
                IsActive = true,
                EmailConfirmed = true
            };

            await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, Input.Password);

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

                await _userManager.AddToRoleAsync(user, nameof(Customer));
                await _appBLL.SaveChangesAsync();
                var customer = new App.BLL.DTO.AdminArea.CustomerDTO
                {
                    AppUserId = user.Id, DisabilityTypeId = Input.DisabilityTypeId.Value
                };
                 _appBLL.Customers.Add(customer);
                await _appBLL.SaveChangesAsync();
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
    ///  Input model 
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Customer first name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(50, MinimumLength = 1)]
        [Display(ResourceType = typeof(CustomerRegister), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// Customer last name
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(CustomerRegister), Name = nameof(LastName))]
        public string LastName { get; set; } = default!;

        /// <summary>
        /// Customer gender
        /// </summary>
        [Display(ResourceType = typeof(CustomerRegister), Name = nameof(Gender))]
        [EnumDataType(typeof(Gender))]
        public Gender Gender { get; set; }

        /// <summary>
        /// Customer date of birth
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Date)]
        [Display(ResourceType = typeof(CustomerRegister), Name = nameof(DateOfBirth))]
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Disability id for customer
        /// </summary>
        [Display(ResourceType = typeof(CustomerRegister), Name = "DisabilityType")]
        public Guid? DisabilityTypeId { get; set; }
        
        /// <summary>
        /// Country id for costumer
        /// </summary>

        [Display(ResourceType = typeof(Register), Name = "Country")]
        public Guid? CountryId { get; set; }

        /// <summary>
        /// County id for customer
        /// </summary>

        [Display(ResourceType = typeof(Register), Name = "County")]
        public Guid? CountyId { get; set; }
        
        /// <summary>
        /// City id for customer
        /// </summary>
        [Display(ResourceType = typeof(Register), Name = "City")]
        public Guid? CityId { get; set; }

        /// <summary>
        /// home address for customer
        /// </summary>
        [Display(ResourceType = typeof(Register), Name = "Address")]
        public string? Address { get; set; }

        /// <summary>
        /// Customer phone number
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(50, ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageMaxLength")]
        [StringLength(50, MinimumLength = 1, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage")]
        [Display(ResourceType = typeof(CustomerRegister), Name = nameof(PhoneNumber))]
        public string? PhoneNumber { get; set; } 

        /// <summary>
        /// Customer email
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageEmail")]
        [Display(ResourceType = typeof(CustomerRegister), Name = nameof(Email))]
        public string Email { get; set; } = default!;

        /// <summary>
        ///  Customer password
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [StringLength(100, ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "StringLengthAttributeErrorMessage",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(CustomerRegister), Name = nameof(Password))]
        public string Password { get; set; } = default!;

        /// <summary>
        ///  Customer password confirm
        /// </summary>
        
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Common), Name = nameof(ConfirmPassword))]
        [Compare(nameof(Password), ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageComparePasswords"
        )]
        public string ConfirmPassword { get; set; } = default!;
    }
}