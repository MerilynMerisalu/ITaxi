using App.Domain.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using WebApp.DTO.Identity;

namespace WebApp.ApiControllers.Identity;

[Route("api/identity/[controller]/[action]")]
[ApiController]
public class AccountController : ControllerBase
{
   private readonly SignInManager<AppUser> _signInManager;
   private readonly UserManager<AppUser> _userManager;
   private readonly IConfiguration _configuration;
   private readonly ILogger<AccountController> _logger;
   private readonly Random _rand = new Random();

   public AccountController(SignInManager<AppUser> signInManager,
      UserManager<AppUser> userManager,
      ILogger<AccountController> logger, IConfiguration configuration)
   {
      _signInManager = signInManager;
      _userManager = userManager;
      _logger = logger;
      _configuration = configuration;
   }

   [HttpPost]
   public async Task<ActionResult<JwtResponse>> LogIn([FromBody] LoginDTO LoginData)
   {
      var appUser = await _userManager.FindByEmailAsync(LoginData.Email);

      if (appUser == null)
      {
         _logger.LogWarning("Webapi login failed! Email {} not found!", LoginData.Email);
         await Task.Delay(_rand.Next(100, 1000));
         return NotFound("Username / password problem!");
      }

      var result = await _signInManager.CheckPasswordSignInAsync(appUser, LoginData.Password,
         false);

      if (!result.Succeeded)
      {
         _logger.LogWarning("Webapi login failed! Password problem for user {}", LoginData.Email);
         await Task.Delay(_rand.Next(100, 1000));
         return NotFound("Username / password problem!");
      }

      var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

      if (claimsPrincipal == null)
      {
         _logger.LogWarning("Could not get claims for user {}!", LoginData.Email);
         await Task.Delay(_rand.Next(100, 1000));
         return NotFound("Username / password problem!");
      }

      var jwt = IdentityExtension.GenerateJwt(
         claimsPrincipal.Claims,
         key: _configuration["JWT:Key"],
         issuer: _configuration["JWT:Issuer"],
         audience: _configuration["JWT:Issuer"],
         expirationDateTime: DateTime.Now.AddDays(_configuration.GetValue<int>("JWT:ExpireInDays")));

      var res = new JwtResponse()
      {
         Token = jwt,
         FirstName = appUser.FirstName,
         LastName = appUser.LastName
      };
      return Ok(res);
   }
}