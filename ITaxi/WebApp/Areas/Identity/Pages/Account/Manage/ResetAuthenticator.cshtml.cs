// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Reset authenticator model 
/// </summary>
public class ResetAuthenticatorModel : PageModel
{
    private readonly ILogger<ResetAuthenticatorModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Reset authenticator model constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="logger">Logger for user's</param>
    public ResetAuthenticatorModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<ResetAuthenticatorModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    /// Status message
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    /// Reset authenticator on get method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        return Page();
    }

    /// <summary>
    /// Reset authenticator on post async method
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await _userManager.SetTwoFactorEnabledAsync(user, false);
        await _userManager.ResetAuthenticatorKeyAsync(user);
        var userId = await _userManager.GetUserIdAsync(user);
        _logger.LogInformation("User with ID '{UserId}' has reset their authentication app key.", user.Id);

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage =
            "Your authenticator app key has been reset, you will need to configure your authenticator app using the new key.";

        return RedirectToPage("./EnableAuthenticator");
    }
}