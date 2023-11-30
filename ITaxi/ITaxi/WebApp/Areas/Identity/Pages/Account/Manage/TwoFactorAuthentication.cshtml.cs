// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Two factor authentication model controller
/// </summary>
public class TwoFactorAuthenticationModel : PageModel
{
    private readonly ILogger<TwoFactorAuthenticationModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Two factor authentication model controller constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="logger">Logger for two factor authentication</param>
    public TwoFactorAuthenticationModel(
        UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
        ILogger<TwoFactorAuthenticationModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    ///  Has authenticator 
    /// </summary>
    public bool HasAuthenticator { get; set; }

    /// <summary>
    ///  Recovery codes left
    /// </summary>
    public int RecoveryCodesLeft { get; set; }

    /// <summary>
    /// Is 2 fa enabled 
    /// </summary>
    [BindProperty]
    public bool Is2faEnabled { get; set; }

    /// <summary>
    /// Is machine remembered  
    /// </summary>
    public bool IsMachineRemembered { get; set; }

    /// <summary>
    /// Status message  
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    /// On get async method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        HasAuthenticator = await _userManager.GetAuthenticatorKeyAsync(user) != null;
        Is2faEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        IsMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);
        RecoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);

        return Page();
    }

    /// <summary>
    /// On post async method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await _signInManager.ForgetTwoFactorClientAsync();
        StatusMessage =
            "The current browser has been forgotten. When you login again from this browser you will be prompted for your 2fa code.";
        return RedirectToPage();
    }
}