// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Confirm email change model
/// </summary>
public class ConfirmEmailChangeModel : PageModel
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Confirm email change model constructor
    /// </summary>
    /// <param name="userManager">Manager for the user's</param>
    /// <param name="signInManager">Sign in manager</param>
    public ConfirmEmailChangeModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    /// <summary>
    /// Status message
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    /// On get async method
    /// </summary>
    /// <param name="userId">User id</param>
    /// <param name="email">User email</param>
    /// <param name="code">Code</param>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGetAsync(string userId, string email, string code)
    {
        if (userId == null || email == null || code == null) return RedirectToPage("/Index");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ChangeEmailAsync(user, email, code);
        if (!result.Succeeded)
        {
            StatusMessage = "Error changing email.";
            return Page();
        }

        // In our UI email and user name are one and the same, so when we update the email
        // we need to update the user name.
        var setUserNameResult = await _userManager.SetUserNameAsync(user, email);
        if (!setUserNameResult.Succeeded)
        {
            StatusMessage = "Error changing user name.";
            return Page();
        }

        await _signInManager.RefreshSignInAsync(user);
        StatusMessage = "Thank you for confirming your email change.";
        return Page();
    }
}