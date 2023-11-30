// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using App.Domain.Identity;
using Base.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Forgot password model controller
/// </summary>
public class ForgotPasswordModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Forgot password model controller constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="emailSender">Email sender</param>
    public ForgotPasswordModel(UserManager<AppUser> userManager, IEmailSender emailSender)
    {
        _userManager = userManager;
        _emailSender = emailSender;
    }

    /// <summary>
    /// Input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    /// On post async method
    /// </summary>
    /// <returns>Redirect to page</returns>
    public async Task<IActionResult> OnPostAsync()
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(Input.Email);
            if (user == null || !await _userManager.IsEmailConfirmedAsync(user))
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToPage("./ForgotPasswordConfirmation");

            // For more information on how to enable account confirmation and password reset please
            // visit https://go.microsoft.com/fwlink/?LinkID=532713
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ResetPassword",
                null,
                new {area = "Identity", code},
                Request.Scheme);

            await _emailSender.SendEmailAsync(
                Input.Email,
                "Reset Password",
                $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl!)}'>clicking here</a>.");

            return RedirectToPage("./ForgotPasswordConfirmation");
        }

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
        [EmailAddress(ErrorMessageResourceType = typeof(Common),
            ErrorMessageResourceName = "ErrorMessageEmail")]
        public string Email { get; set; } = default!;
    }
}