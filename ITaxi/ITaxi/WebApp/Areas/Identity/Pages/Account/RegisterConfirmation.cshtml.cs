// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.Text;
using App.Domain.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Register confirmation model
/// </summary>
[AllowAnonymous]
public class RegisterConfirmationModel : PageModel
{
    private readonly IEmailSender _sender;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Register confirmation model constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="sender">Sender</param>
    public RegisterConfirmationModel(UserManager<AppUser> userManager, IEmailSender sender)
    {
        _userManager = userManager;
        _sender = sender;
    }

    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Display confirm account link
    /// </summary>
    public bool DisplayConfirmAccountLink { get; set; }

    /// <summary>
    /// Email confirmation url
    /// </summary>
    public string EmailConfirmationUrl { get; set; }

    /// <summary>
    /// Register confirmation on get async method
    /// </summary>
    /// <param name="email">Email</param>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
    {
        if (email == null) return RedirectToPage("/Index");
        returnUrl = returnUrl ?? Url.Content("~/");

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound($"Unable to load user with email '{email}'.");

        Email = email;
        // Once you add a real email sender, you should remove this code that lets you confirm the account
        DisplayConfirmAccountLink = true;
        if (DisplayConfirmAccountLink)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            EmailConfirmationUrl = Url.Page(
                "/Account/ConfirmEmail",
                null,
                new {area = "Identity", userId, code, returnUrl},
                Request.Scheme);
        }

        return Page();
    }
}