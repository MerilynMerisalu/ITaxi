// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Email model
/// </summary>
public class EmailModel : PageModel
{
    private readonly IEmailSender _emailSender;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Email model constructor
    /// </summary>
    /// <param name="userManager">Manager for the user's</param>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="emailSender">Email sender</param>
    public EmailModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailSender = emailSender;
    }

    /// <summary>
    /// Email model email
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Email model is email confirmed
    /// </summary>
    public bool IsEmailConfirmed { get; set; }

    /// <summary>
    /// Email model email status message
    /// </summary>
    [TempData]
    public string StatusMessage { get; set; }

    /// <summary>
    /// Email model input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    private async Task LoadAsync(AppUser user)
    {
        var email = await _userManager.GetEmailAsync(user);
        Email = email;

        Input = new InputModel
        {
            NewEmail = email
        };

        IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);
    }

    /// <summary>
    /// Email model on get async method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        await LoadAsync(user);
        return Page();
    }

    /// <summary>
    /// Email model on post change email async method
    /// </summary>
    /// <returns>Redirect to page</returns>
    public async Task<IActionResult> OnPostChangeEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var email = await _userManager.GetEmailAsync(user);
        if (Input.NewEmail != email)
        {
            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateChangeEmailTokenAsync(user, Input.NewEmail);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmailChange",
                null,
                new {area = "Identity", userId, email = Input.NewEmail, code},
                Request.Scheme);
            await _emailSender.SendEmailAsync(
                Input.NewEmail,
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

            StatusMessage = "Confirmation link to change email sent. Please check your email.";
            return RedirectToPage();
        }

        StatusMessage = "Your email is unchanged.";
        return RedirectToPage();
    }

    /// <summary>
    /// Email model on post send verification email async method
    /// </summary>
    /// <returns>Redirect to page</returns>
    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var userId = await _userManager.GetUserIdAsync(user);
        var email = await _userManager.GetEmailAsync(user);
        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        var callbackUrl = Url.Page(
            "/Account/ConfirmEmail",
            null,
            new {area = "Identity", userId, code},
            Request.Scheme);
        await _emailSender.SendEmailAsync(
            email,
            "Confirm your email",
            $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        StatusMessage = "Verification email sent. Please check your email.";
        return RedirectToPage();
    }

    /// <summary>
    ///  Email input model
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Email input model new email
        /// </summary>
        [Required]
        [EmailAddress]
        [Display(Name = "New email")]
        public string NewEmail { get; set; }
    }
}