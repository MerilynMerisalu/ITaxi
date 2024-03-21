// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Login with 2 fa model
/// </summary>
public class LoginWith2faModel : PageModel
{
    private readonly ILogger<LoginWith2faModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Login with 2 fa model constructor
    /// </summary>
    /// <param name="signInManager">Manager for sign in</param>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="logger">Logger for user's</param>
    public LoginWith2faModel(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<LoginWith2faModel> logger)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    /// Remember me
    /// </summary>
    public bool RememberMe { get; set; }

    /// <summary>
    /// Return url
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// Login with 2 fa model on get async method
    /// </summary>
    /// <param name="rememberMe">Remember me</param>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Page</returns>
    /// <exception cref="InvalidOperationException">Invalid operation exception</exception>
    public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

        if (user == null) throw new InvalidOperationException("Unable to load two-factor authentication user.");

        ReturnUrl = returnUrl;
        RememberMe = rememberMe;

        return Page();
    }

    /// <summary>
    /// Login with 2 fa model on post async method
    /// </summary>
    /// <param name="rememberMe">Remember me</param>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Page</returns>
    /// <exception cref="InvalidOperationException">Invalid operation exception</exception>
    public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        returnUrl = returnUrl ?? Url.Content("~/");

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null) throw new InvalidOperationException("Unable to load two-factor authentication user.");

        var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

        var result =
            await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe,
                Input.RememberMachine);

        var userId = await _userManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
            return LocalRedirect(returnUrl);
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
            return RedirectToPage("./Lockout");
        }

        _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
        ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
        return Page();
    }

    /// <summary>
    /// Input model
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Two factor code
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }

        /// <summary>
        ///  Remember machine
        /// </summary>
        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }
    }
}