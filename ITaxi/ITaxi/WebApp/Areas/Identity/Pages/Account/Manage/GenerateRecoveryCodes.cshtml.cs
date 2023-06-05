// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Generate Recovery codes model
/// </summary>
public class GenerateRecoveryCodesModel : PageModel
{
    private readonly ILogger<GenerateRecoveryCodesModel> _logger;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Generate Recovery codes model constructor
    /// </summary>
    /// <param name="userManager">Manager for the user's</param>
    /// <param name="logger">Logger for the user's</param>
    public GenerateRecoveryCodesModel(
        UserManager<AppUser> userManager,
        ILogger<GenerateRecoveryCodesModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Recovery codes
    /// </summary>
    [TempData]
    public string[] RecoveryCodes { get; set; }

    /// <summary>
    /// Status message
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    /// On get async method
    /// </summary>
    /// <returns>Page</returns>
    /// <exception cref="InvalidOperationException">Invalid operation exception</exception>
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        if (!isTwoFactorEnabled)
            throw new InvalidOperationException(
                "Cannot generate recovery codes for user because they do not have 2FA enabled.");

        return Page();
    }

    /// <summary>
    /// On post async method
    /// </summary>
    /// <returns>Page</returns>
    /// <exception cref="InvalidOperationException">Invalid operation exception</exception>
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
        var userId = await _userManager.GetUserIdAsync(user);
        if (!isTwoFactorEnabled)
            throw new InvalidOperationException(
                "Cannot generate recovery codes for user as they do not have 2FA enabled.");

        var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        RecoveryCodes = recoveryCodes.ToArray();

        _logger.LogInformation("User with ID '{UserId}' has generated new 2FA recovery codes.", userId);
        StatusMessage = "You have generated new recovery codes.";
        return RedirectToPage("./ShowRecoveryCodes");
    }
}