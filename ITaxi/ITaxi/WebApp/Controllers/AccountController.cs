using App.DAL.DTO.Identity;
using App.Enum.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[AllowAnonymous, Route("Account")]
public class AccountController : Controller
{
    private UserManager<AppUser> userManager;
    private SignInManager<AppUser> signInManager;

    public AccountController(UserManager<AppUser> userMgr, SignInManager<AppUser> signinMgr)
    {
        userManager = userMgr;
        signInManager = signinMgr;
    }

    // other methods

    public IActionResult AccessDenied()
    {
        return View();
    }

    [Route("google-login")]
    public IActionResult GoogleLogin()
    {
        string redirectUrl = Url.Action("GoogleResponse", "Account");
        var properties = signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
        return new ChallengeResult("Google", properties);
    }

    public IActionResult Login()
    {
        return View();
    }

    [AllowAnonymous]
    public async Task<IActionResult> GoogleResponse()
    {
        ExternalLoginInfo info = await signInManager.GetExternalLoginInfoAsync();
        if (info == null)
            return RedirectToAction(nameof(Login));

        var result = await signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, false);
        string[] userInfo = { info.Principal.FindFirst(ClaimTypes.Name).Value, info.Principal.FindFirst(ClaimTypes.Email).Value };
        if (result.Succeeded)
            return View(userInfo);
        else
        {
            AppUser user = new AppUser
            {
                Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                FirstName = info.Principal.FindFirst(ClaimTypes.GivenName).Value,
                LastName = info.Principal.FindFirst(ClaimTypes.Surname).Value,
                Gender = Enum.Parse<Gender>(info.Principal.FindFirst(ClaimTypes.Gender).Value),
                DateOfBirth = DateTime.Parse(info.Principal.FindFirst(ClaimTypes.DateOfBirth).Value).Date,
                PhoneNumber = info.Principal.FindFirst(ClaimTypes.MobilePhone).Value,
                
            };

            IdentityResult identResult = await userManager.CreateAsync(user);
            if (identResult.Succeeded)
            {
                identResult = await userManager.AddLoginAsync(user, info);
                if (identResult.Succeeded)
                {
                    await signInManager.SignInAsync(user, false);
                    return View(userInfo);
                }
            }
            return AccessDenied();
        }
    }
}