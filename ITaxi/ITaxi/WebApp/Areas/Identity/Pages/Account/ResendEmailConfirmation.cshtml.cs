// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using App.Domain.Identity;
using Base.Resources;
using Base.Resources.Identity.Pages.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Resend email confirmation model controller
/// </summary>
[AllowAnonymous]
public class ResendEmailConfirmationModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Resend email confirmation model constructor
    /// </summary>
    /// <param name="userManager">Manager for user</param>
    /// <param name="emailSender">Email sender</param>
    public ResendEmailConfirmationModel(UserManager<AppUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    /// On get method
    /// </summary>
    public void OnGet()
    {
    }

    /// <summary>
    /// On post async method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var user = await _userManager.FindByEmailAsync(Input.Email);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, ResendEmailConfirmation.VerificationEmail);
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            null,
            new {userId, code},
            Request.Scheme);
        await _emailSender.SendEmailAsync(
            Input.Email,
            "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        ModelState.AddModelError(string.Empty, ResendEmailConfirmation.VerificationEmail);
        return Page();
    }

    /// <summary>
    ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
    ///     directly from your code. This API may change or be removed in future releases.
    /// </summary>
    public class InputModel
    {
        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [Required(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "RequiredAttributeErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(Common), ErrorMessageResourceName = "ErrorMessageEmail")]
        public string Email { get; set; }
    }
}