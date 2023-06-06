// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebApp.Areas.Identity.Pages.Account.Manage;

/// <summary>
/// Personal data model
/// </summary>
public class PersonalDataModel : PageModel
{
    private readonly ILogger<PersonalDataModel> _logger;
    private readonly UserManager<AppUser> _userManager;

    /// <summary>
    /// Personal data model constructor
    /// </summary>
    /// <param name="userManager">Manager for user's</param>
    /// <param name="logger">Logger for user's</param>
    public PersonalDataModel(
        UserManager<AppUser> userManager,
        ILogger<PersonalDataModel> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }

    /// <summary>
    /// Personal data on get method
    /// </summary>
    /// <returns>Page</returns>
    public async Task<IActionResult> OnGet()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");

        return Page();
    }
}