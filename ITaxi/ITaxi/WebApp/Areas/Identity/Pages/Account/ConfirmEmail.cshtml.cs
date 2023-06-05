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
/// Confirm email model
/// </summary>
public class ConfirmEmailModel : PageModel
{
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Confirm email model constructor
    /// </summary>
    /// <param name="userManager">Manager for the user's</param>
    public ConfirmEmailModel(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
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
    /// <param name="code">Code</param>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGetAsync(string userId, string code)
    {
        if (userId == null || code == null) return RedirectToPage("/Index");

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return NotFound($"Unable to load user with ID '{userId}'.");

        code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
        var result = await _userManager.ConfirmEmailAsync(user, code);
        StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
        return Page();
    }
}