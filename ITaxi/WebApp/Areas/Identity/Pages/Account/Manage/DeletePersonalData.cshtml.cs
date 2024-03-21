// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using System.ComponentModel.DataAnnotations;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Delete personal data model
/// </summary>
public class DeletePersonalDataModel : PageModel
{
    private readonly ILogger<DeletePersonalDataModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Delete personal data model constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="signInManager">Sign in manager</param>
    /// <param name="logger">Logger for user's</param>
    public DeletePersonalDataModel(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        ILogger<DeletePersonalDataModel> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    /// Input
    /// </summary>
    [BindProperty]
    public InputModel Input { get; set; }

    /// <summary>
    /// Require password
    /// </summary>
    public bool RequirePassword { get; set; }

    /// <summary>
    /// Delete personal data on get method
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        RequirePassword = await _userManager.HasPasswordAsync(user);
        return Page();
    }

    /// <summary>
    /// Delete personal data on post method
    /// </summary>
    /// <returns>Page</returns>
    /// <exception cref="InvalidOperationException">Invalid operation exception</exception>
    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        RequirePassword = await _userManager.HasPasswordAsync(user);
        if (RequirePassword)
            if (!await _userManager.CheckPasswordAsync(user, Input.Password))
            {
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return Page();
            }

        var result = await _userManager.DeleteAsync(user);
        var userId = await _userManager.GetUserIdAsync(user);
        if (!result.Succeeded) throw new InvalidOperationException("Unexpected error occurred deleting user.");

        await _signInManager.SignOutAsync();

        _logger.LogInformation("User with ID '{UserId}' deleted themselves.", userId);

        return Redirect("~/");
    }

    /// <summary>
    /// Input model
    /// </summary>
    public class InputModel
    {
        /// <summary>
        /// Password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}