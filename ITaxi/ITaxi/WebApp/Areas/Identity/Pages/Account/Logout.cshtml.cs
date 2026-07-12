// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

#nullable disable

using App.Domain.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account;

/// <summary>
/// Logout model
/// </summary>
public class LogoutModel : PageModel
{
    private readonly ILogger<LogoutModel> _logger;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    /// <summary>
    /// Logout model constructor
    /// </summary>
    /// <param name="signInManager">Manager for sign in</param>
    /// <param name="logger">Logger for user's</param>
    public LogoutModel(SignInManager<AppUser> signInManager, ILogger<LogoutModel> logger, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _logger = logger;
        _userManager = userManager;
    }

    /// <summary>
    /// Logout on post method
    /// </summary>
    /// <param name="returnUrl">Return url</param>
    /// <returns>Redirect to page</returns>
    public async Task<IActionResult> OnPost(string returnUrl = null)
    {
        var user = await _userManager.GetUserAsync(User);
        if (User.Identity.IsAuthenticated && user != null)
        {
            user.IsActive = false;
            await _userManager.UpdateAsync(user);
        }
       
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out.");
        if (returnUrl != null)
            return LocalRedirect(returnUrl);
        return RedirectToPage();
    }

    
}