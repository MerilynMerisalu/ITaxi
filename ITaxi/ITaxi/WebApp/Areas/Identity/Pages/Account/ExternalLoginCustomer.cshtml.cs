// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using App.Domain.Identity;
using App.Enum.Enum;
using App.Resources.Areas.Identity.Pages.Account.Manage;
using Base.Resources;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using WebApp.ApiControllers.Identity;
using WebApp.Controllers;
using Microsoft.AspNetCore.Authentication;
using Google.Apis;
using Google.Apis.PeopleService.v1;
using Index = System.Index;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using App.Domain;
using App.DAL.EF;
using WebApp.Filters;
using NuGet.Packaging;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// External login model
/// </summary>
[AllowAnonymous]
public class ExternalLoginCustomerModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly IUserEmailStore<AppUser> _emailStore;
    private readonly ILogger<ExternalLoginCustomerModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IUserStore<AppUser> _userStore;
    private readonly HttpContext _httpContext;
    private readonly AppDbContext _appDbContext;
    /// <summary>
    /// External login model constructor
    /// </summary>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="userManager">Manager for the user's</param>
    /// <param name="userStore">Store for the user's</param>
    /// <param name="logger">Logger for the user's</param>
    /// <param name="emailSender">Email sender</param>
    public ExternalLoginCustomerModel(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IUserStore<AppUser> userStore,
        ILogger<ExternalLoginCustomerModel> logger,
        IEmailSender emailSender,
        AppDbContext appDbContext)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _userStore = userStore;
        _emailStore = GetEmailStore();
        _logger = logger;
        _emailSender = emailSender;
        _appDbContext = appDbContext;
    }

    /// <summary>
    /// Input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    /// Provider display name
    /// </summary>
    public string ProviderDisplayName { get; set; }

    /// <summary>
    /// Return url
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// Error message
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; }

    /// <summary>
    /// On get method
    /// </summary>
    /// <returns>Page</returns>
    public IActionResult OnGet()
    {
        return RedirectToPage("./Login");
    }

    /// <summary>
    /// On post method
    /// </summary>
    /// <param name="provider">Provider</param>
    /// <param name="returnUrl">Return url</param>
    /// <returns>New challenge result</returns>

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
        // Request a redirect to the external login provider.
        var redirectUrl = Url.Page("./ExternalLoginCustomer", "Callback", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return new ChallengeResult(provider, properties);
    }
    //public IActionResult OnPostDriver(string provider, string returnUrl = null)
    //{
    //    // Request a redirect to the external login provider.
    //    var redirectUrl = Url.Page("./ExternalLogin", "CallbackDriver", new { returnUrl });
    //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
    //    return new ChallengeResult(provider, properties);
    //}

    /// <summary>
    /// On get callback async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    /// <param name="remoteError">remote error</param>
    /// <returns>Page</returns>
    /*public async Task<IActionResult> OnGetCallbackDriverAsync(string returnUrl = null, string remoteError = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }


        // create account
        if (await RegisterUserAsync("Driver"))
        {
            // redirect to home
            return Redirect("/");
        }

        return null;
    }
    */

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        if (remoteError != null)
        {
            ErrorMessage = $"Error from external provider: {remoteError}";
            return RedirectToPage("./Login", new {ReturnUrl = returnUrl});
        }


        var info = await _signInManager.GetExternalLoginInfoAsync();
        string disabilityTypeId = null;
        if (info == null)
        {
            ErrorMessage = "Error loading external login information.";
            return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
        }

        // Sign in the user with this external login provider if the user already has a login.
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false, true);
        if (result.Succeeded)
        {
            _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name,
                info.LoginProvider);
            return LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut) return RedirectToPage("./Lockout");

        // If the user does not have an account, then ask the user to create an account.
        ReturnUrl = returnUrl;
        ProviderDisplayName = info.ProviderDisplayName;

        //var accessToken = info.AuthenticationTokens.FirstOrDefault(x => x.Name == "access_token")?.Value;

        //// Retrieve the access token from the authentication properties
        ////var accessToken = await _httpContext.GetTokenAsync("access_token");

        //// Use the token to make an API call to the Google People API, for example
        /*var peopleService = new PeopleServiceService(new BaseClientService.Initializer
        {
            HttpClientInitializer = new UserCredential(null, "user", new TokenResponse { AccessToken = accessToken }),
            ApplicationName = "ITaxi",
        });*/

        //// Request detailed user info from the People API
        //var request = peopleService.People.Get("people/me");
        //request.PersonFields = "genders,birthdays,phoneNumbers";
        //var person = await request.ExecuteAsync();

        //// Now you can access additional user details
        //var gender = person.Genders?.FirstOrDefault()?.Value;
        //var birthday = person.Birthdays?.FirstOrDefault()?.Date;
        //var phoneNumber = person.PhoneNumbers?.FirstOrDefault()?.Value;

        // create account
        if (await RegisterUserAsync("Customer"))
        {
            // add to context customers
            // redirect to profile
            return Redirect("/Identity/Account/Manage/Index");
           
            
            
        }
        string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value ,
            info.Principal.FindFirst(ClaimTypes.Gender).Value,
            info.Principal.FindFirst(ClaimTypes.DateOfBirth).Value,
            info.Principal.FindFirst(ClaimTypes.MobilePhone).Value,
        disabilityTypeId};
        // login
        // show profile page or homepage
        
        
        
        

        if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
            Input = new InputModel
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                //Gender = Enum.Parse<Gender>(gender, true),
                //DateOfBirth = new DateTime(birthday.Year.Value, birthday.Month.Value, birthday.Day.Value), // DateTime.Parse($"{birthday.Year}-{birthday.Month}-{birthday.Day}")
                //PhoneNumber = phoneNumber
            };
        return Page();
    }

    private async Task<bool> RegisterUserAsync(string role)
    {


        //////
        ///

        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return false;
        // Retrieve the access token from the authentication properties
        var accessToken = info.AuthenticationTokens.FirstOrDefault(a => a.Name == "access_token").Value;
        var credentials = GoogleCredential.FromAccessToken(accessToken);
        // Use the token to make an API call to the Google People API, for example
        var peopleService = new PeopleServiceService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials,
            ApplicationName = "ITaxi",
        });

        // Request detailed user info from the People API
        var request = peopleService.People.Get("people/me");
        request.PersonFields = "genders,birthdays,phoneNumbers";
        var person = request.Execute(); // make this async

        // Now you can access additional user details
        var gender = person.Genders?.FirstOrDefault()?.Value;
        var birthday = person.Birthdays?.FirstOrDefault()?.Date;
        var phoneNumber = person.PhoneNumbers.FirstOrDefault()?.Value;
        var email = info.Principal.FindFirst(ClaimTypes.Email).Value;
        var firstName = info.Principal.FindFirstValue(ClaimTypes.GivenName);
        var lastName = info.Principal.FindFirstValue(ClaimTypes.Surname);
       
        //////

        var user = CreateUser();

        user.FirstName = firstName;
        user.LastName = lastName;
        user.Gender = Enum.Parse<Gender>(gender, true);
        user.DateOfBirth = new DateTime(birthday.Year.Value, birthday.Month.Value, birthday.Day.Value); // DateTime.Parse($"{birthday.Year}-{birthday.Month}-{birthday.Day}")
        user.PhoneNumber = phoneNumber;

        await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, email, CancellationToken.None);

        
        var result = await _userManager.CreateAsync(user);
        if (result.Succeeded)
        {
            await _userManager.AddClaimAsync(user, new Claim("aspnet.firstname", firstName));
            await _userManager.AddClaimAsync(user, new Claim("aspnet.lastname", lastName));

            await _userManager.AddToRoleAsync(user, role);
            result = await _userManager.AddLoginAsync(user, info);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    null,
                    new { area = "Identity", userId, code },
                    Request.Scheme);
                
                //await _emailSender.SendEmailAsync(email, "Confirm your email",
                //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
                var customer = new Customer() { AppUser = user,
                CreatedAt = DateTime.Now.ToUniversalTime(),
                UpdatedAt = DateTime.Now.ToUniversalTime()};
                await _appDbContext.Customers.AddAsync(customer);
                await _appDbContext.SaveChangesAsync();
                await _signInManager.SignInAsync(user, false, info.LoginProvider);
                
                return true;
            }
        }

        foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

        return false;
    }

    /// <summary>
    /// On post confirmation async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = "/Home/Index")
    {
        returnUrl = returnUrl ?? Url.Content("~/");
        // Get the information about the user from the external login provider
        var info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
        {
            ErrorMessage = "Error loading external login information during confirmation.";
            return RedirectToPage("./Login", new {ReturnUrl = returnUrl});
        }

        if (ModelState.IsValid)
        {
            await RegisterUserAsync("");
        }

        ProviderDisplayName = info.ProviderDisplayName;
        ReturnUrl = returnUrl;
        return Page();
    }
    public IActionResult GoogleLogin()
    {
        string redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return new ChallengeResult("Google", properties);
    }

    /// <summary>
    /// A method for handling a Google response
    /// </summary>
    /// <returns>An IActionResult</returns>
    public async Task<IActionResult> GoogleResponse()
    {
        ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction("Login", nameof(AccountController));
        // Retrieve the access token from the authentication properties
        var accessToken = info.AuthenticationTokens.FirstOrDefault(a => a.Name == "access_token").Value;
        var credentials = GoogleCredential.FromAccessToken(accessToken);
        // Use the token to make an API call to the Google People API, for example
        var peopleService = new PeopleServiceService(new BaseClientService.Initializer
        {
            HttpClientInitializer = credentials,
            ApplicationName = "ITaxi",
        });

        // Request detailed user info from the People API
        var request = peopleService.People.Get("people/me");
        request.PersonFields = "genders,birthdays,phoneNumbers";
        var person = request.Execute(); // make this async

        // Now you can access additional user details
        var gender = person.Genders?.FirstOrDefault()?.Value;
        var birthday = person.Birthdays?.FirstOrDefault()?.Date;
        var phoneNumber = person.PhoneNumbers?.FirstOrDefault()?.Value;
        string disabilityTypeId = null;
        var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        

        
        if (result.Succeeded)
            return RedirectToPage(nameof(Index), nameof(HomeController));
        else
        {
            
            
            AppUser user = new AppUser
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                FirstName = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                LastName = info.Principal.FindFirst(ClaimTypes.Surname).Value,
               // Gender = Enum.Parse<Gender>(info.Principal.FindFirst(ClaimTypes.Gender).Value),
                //DateOfBirth = DateTime.Parse(info.Principal.FindFirstValue(ClaimTypes.DateOfBirth)).Date,
                PhoneNumber = info.Principal.FindFirstValue(ClaimTypes.MobilePhone)
            };

            IdentityResult identResult = await _userManager.CreateAsync(user);
            if (identResult.Succeeded)
            {
                identResult = await _userManager.AddLoginAsync(user, info);
                if (identResult.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                    return RedirectToPage(nameof(Index), nameof(HomeController));
                }
            }
            return Page();
        }
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
                                                "override the external login page in /Areas/Identity/Pages/Account/ExternalLoginCustomer.cshtml");
        }
    }

    private IUserEmailStore<AppUser> GetEmailStore()
    {
        if (!_userManager.SupportsUserEmail)
            throw new NotSupportedException("The default UI requires a user store with email support.");
        return (IUserEmailStore<AppUser>) _userStore;
    }

    /// <summary>
    /// Input model
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(ResourceType = typeof(ExternalLogin), Name = nameof(Email))]
        public string Email { get; set; }
        /// <summary>
        /// First Name
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(ExternalLogin), Name = nameof(FirstName))]
        public string FirstName { get; set; } = default!;

        /// <summary>
        /// Last Name
        /// </summary>
        [Required]
        [Display(ResourceType = typeof(ExternalLogin), Name = nameof(LastName))]
        public string LastName { get; set; } = default!;

        /// <summary>
        /// Gender
        /// </summary>
        [Required]
        public Gender Gender { get; set; } = default!;

        /// <summary>
        /// Date of Birth
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        /// <summary>
        /// Phone Number
        /// </summary>
        [Required]
        [Phone]
        [Display(ResourceType = typeof(ExternalLogin), Name = nameof(PhoneNumber))]
        public string PhoneNumber { get; set; } = default!;
    }
}