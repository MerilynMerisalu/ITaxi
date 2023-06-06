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
/// Login with recovery code model
/// </summary>
public class LoginWithRecoveryCodeModel : PageModel
{
    private readonly ILogger<LoginWithRecoveryCodeModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Login with recovery code model constructor
    /// </summary>
    /// <param name="signInManager">Manager for sign in</param>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="logger">Logger for user's</param>
    public LoginWithRecoveryCodeModel(
        SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        ILogger<LoginWithRecoveryCodeModel> logger)
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
    /// Return url
    /// </summary>
    public string ReturnUrl { get; set; }

    /// <summary>
    /// Login with recovery code on get async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Page</returns>
    /// <exception cref="InvalidOperationException">Invalid operation exception</exception>
    public async Task<IActionResult> OnGetAsync(string returnUrl = null)
    {
        // Ensure the user has gone through the username & password screen first
        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null) throw new InvalidOperationException("Unable to load two-factor authentication user.");

        ReturnUrl = returnUrl;

        return Page();
    }

    /// <summary>
    /// Login with recovery code on post async method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Page</returns>
    /// <exception cref="InvalidOperationException">Invalid operation exception</exception>
    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        if (!ModelState.IsValid) return Page();

        var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
        if (user == null) throw new InvalidOperationException("Unable to load two-factor authentication user.");

        var recoveryCode = Input.RecoveryCode.Replace(" ", string.Empty);

        var result = await _signInManager.TwoFactorRecoveryCodeSignInAsync(recoveryCode);

        var userId = await _userManager.GetUserIdAsync(user);

        if (result.Succeeded)
        {
            _logger.LogInformation("User with ID '{UserId}' logged in with a recovery code.", user.Id);
            return LocalRedirect(returnUrl ?? Url.Content("~/"));
        }

        if (result.IsLockedOut)
        {
            _logger.LogWarning("User account locked out.");
            return RedirectToPage("./Lockout");
        }

        _logger.LogWarning("Invalid recovery code entered for user with ID '{UserId}' ", user.Id);
        ModelState.AddModelError(string.Empty, "Invalid recovery code entered.");
        return Page();
    }

    /// <summary>
    /// Input model
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Recovery code
        /// </summary>
        [BindProperty]
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Recovery Code")]
        public string RecoveryCode { get; set; }
    }
}