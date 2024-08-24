// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable enable

using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Base.Resources;
using Base.Resources.Identity.Pages.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Login model
/// </summary>
public class LoginModel : PageModel
{
    private readonly ILogger<LoginModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;

    /// <summary>
    /// Login model constructor
    /// </summary>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="logger">Logger for the user's</param>
    public LoginModel(SignInManager<AppUser> signInManager, ILogger<LoginModel> logger)
    {
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    /// Input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; } = default!;

    /// <summary>
    /// External logins
    /// </summary>
    public IList<AuthenticationScheme>? ExternalLogins { get; set; }

    /// <summary>
    /// Return url
    /// </summary>
    public string? ReturnUrl { get; set; }

    /// <summary>
    /// Error message
    /// </summary>
    [TempData]
    public string ErrorMessage { get; set; } = default!;

    /// <summary>
    /// On get async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    public async Task OnGetAsync(string? returnUrl = null)
    {
        if (!string.IsNullOrEmpty(ErrorMessage)) ModelState.AddModelError(string.Empty, ErrorMessage);

        returnUrl ??= Url.Content("~/");

        // Clear the existing external cookie to ensure a clean login process
        await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        ReturnUrl = returnUrl;
    }

    /// <summary>
    /// On post async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnPostAsync(string? returnUrl = null)
    {
        returnUrl ??= Url.Content("~/");

        ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        if (ModelState.IsValid)
        {
            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, set lockoutOnFailure: true
            var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, Input.RememberMe, false);
            if (result.Succeeded)
            {
                _logger.LogInformation("User logged in.");
                return LocalRedirect(returnUrl);
            }

            if (result.RequiresTwoFactor)
                return RedirectToPage("./LoginWith2fa", new {ReturnUrl = returnUrl, Input.RememberMe});
            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account locked out.");
                return RedirectToPage("./Lockout");
            }

            ModelState.AddModelError(string.Empty, Login.InvalidLoginAttempt);
            return Page();
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    /// <summary>
    /// Input model
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(Common)
            , ErrorMessageResourceName = "ErrorMessageEmail")]
        [Display(ResourceType = typeof(Login), Name = nameof(Email))]
        public string Email { get; set; } = default!;

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common)
            , ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [DataType(DataType.Password)]
        [Display(ResourceType = typeof(Login), Name = nameof(Password))]
        public string Password { get; set; } = default!;
        /// <summary>
        /// Show / hide password
        /// </summary>
        public bool ShowPassword { get; set; } = default;

        /// <summary>
        /// Remember me
        /// </summary>
        [Display(ResourceType = typeof(Login), Name = nameof(RememberMe))]
        public bool RememberMe { get; set; }

    }
}