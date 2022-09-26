using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.Contracts.DAL;
using App.DAL.EF;
using App.Domain;
using App.Domain.DTO;
using App.Domain.Identity;
using App.Resources.Areas.Identity.Pages.Account;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.MSIdentity.Shared;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;
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
   //private readonly IAppUnitOfWork _uow;
#warning Code smell, fix it!
   private readonly AppDbContext _context;


   public AccountController(SignInManager<AppUser> signInManager,
      UserManager<AppUser> userManager,
      ILogger<AccountController> logger, IConfiguration configuration, IAppUnitOfWork uow, AppDbContext context)
   {
      _signInManager = signInManager;
      _userManager = userManager;
      _logger = logger;
      _configuration = configuration;
      //_uow = uow;
      _context = context;
   }

   public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModelDTO refreshTokenModelDTO)
   {
      JwtSecurityToken jwtToken;
      try
      {
         jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModelDTO.Token);
         if (jwtToken == null)
         {
            return BadRequest("No token");
         }

      }
      catch (Exception e)
      {
         return BadRequest($"Cannot parse the token {e.Message}");
      }
       #warning  Validate token signature

      var userEmail = jwtToken.Claims.FirstOrDefault(e => e.Type.Equals(ClaimTypes.Email))?.Value;
      if (userEmail == null)
      {
         return BadRequest("No email in jwt");
      }

      var appUser = await _userManager.FindByEmailAsync(userEmail);
      if (appUser == null)
      {
         return BadRequest($"User with email ${userEmail} was not found!");
      }

      await _context.Entry(appUser).Collection(c => c.RefreshTokens!)
         .Query().Where(t => (t.Token == refreshTokenModelDTO.RefreshToken
                             && t.TokenExpirationDateAndTime > DateTime.UtcNow) ||
                             t.PreviousToken == refreshTokenModelDTO.RefreshToken
                             && t.PreviousTokenExpirationDateAndTime > DateTime.UtcNow).ToListAsync();
      
      if (appUser.RefreshTokens == null)
      {
         return Problem("Refresh token collection is null");
      }

      if (appUser.RefreshTokens.Count == 0)
      {
         return Problem("Refresh token collection is empty! No valid refresh tokens found!");
      }
      if (appUser.RefreshTokens!.Count != 1)
      {
         return Problem("More than one valid refresh token found");
      }
      var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

      if (claimsPrincipal == null)
      {
         _logger.LogWarning("Could not get claims for user {}!", userEmail);
         await Task.Delay(_rand.Next(100, 1000));
         return NotFound("Username / password problem!");
      }

      var jwt = IdentityExtension.GenerateJwt(
         claimsPrincipal.Claims,
         key: _configuration["JWT:Key"],
         issuer: _configuration["JWT:Issuer"],
         audience: _configuration["JWT:Issuer"],
         expirationDateTime: DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")));

      var refreshToken = appUser.RefreshTokens.First();
      if (refreshToken.Token == refreshTokenModelDTO.Token)
      {
         refreshToken.PreviousToken = refreshToken.Token;
         refreshToken.PreviousTokenExpirationDateAndTime = DateTime.UtcNow.AddMinutes(5);

         refreshToken.Token = Guid.NewGuid().ToString();
         refreshToken.TokenExpirationDateAndTime = DateTime.UtcNow.AddDays(7);

         await _context.SaveChangesAsync();
      }
      
      var res = new JwtResponse()
      {
         Token = jwt,
         RefreshToken = refreshToken.Token,
         FirstName = appUser.FirstName,
         LastName = appUser.LastName
      };
      
      return Ok(res);
   }

   [HttpPost]
   public async Task<ActionResult<JwtResponse>> LogIn([FromBody] LoginDTO loginData)
   {
      var appUser = await _userManager.FindByEmailAsync(loginData.Email);

      if (appUser == null)
      {
         _logger.LogWarning("Webapi login failed! Email {} not found!", loginData.Email);
         await Task.Delay(_rand.Next(100, 1000));
         return NotFound("Username / password problem!");
      }

      var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password,
         false);

      if (!result.Succeeded)
      {
         _logger.LogWarning("Webapi login failed! Password problem for user {}", loginData.Email);
         await Task.Delay(_rand.Next(100, 1000));
         return NotFound("Username / password problem!");
      }

      var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

      if (claimsPrincipal == null)
      {
         _logger.LogWarning("Could not get claims for user {}!", loginData.Email);
         await Task.Delay(_rand.Next(100, 1000));
         return NotFound("Username / password problem!");
      }

      appUser.RefreshTokens = await _context
         .Entry(appUser)
         .Collection(a => a.RefreshTokens!)
         .Query()
         .Where(t => t.AppUserId == appUser.Id)
         .ToListAsync();

      foreach (var userRefreshToken in appUser.RefreshTokens)
      {
         if (userRefreshToken.TokenExpirationDateAndTime < DateTime.UtcNow &&
             userRefreshToken.PreviousTokenExpirationDateAndTime < DateTime.UtcNow)
         {
            _context.RefreshTokens.Remove(userRefreshToken);
         }
      }
        
      var refreshToken = new RefreshToken();
      refreshToken.AppUserId = appUser.Id;
      _context.RefreshTokens.Add(refreshToken);

      await _context.SaveChangesAsync();
      var jwt = IdentityExtension.GenerateJwt(
         claimsPrincipal.Claims,
         key: _configuration["JWT:Key"],
         issuer: _configuration["JWT:Issuer"],
         audience: _configuration["JWT:Issuer"],
         expirationDateTime: DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")));

      
      var res = new JwtResponse()
      {
         Token = jwt,
         FirstName = appUser.FirstName,
         LastName = appUser.LastName,
         RefreshToken = refreshToken.Token
      };
      return Ok(res);
   }

   #warning Is that the right way to do in my app context?
   
    [HttpPost]
    [Authorize(Roles = "Admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme )]
   public async Task<ActionResult<JwtResponseAdminRegister>> RegisterAdmin(AdminRegistrationDTO adminRegister)
   {
      var appUser = await _userManager.FindByEmailAsync(adminRegister.Email);
      if (appUser != null)
      {
         _logger.LogWarning("Webapi user registration failed! User with an email {} already exist!", 
            adminRegister.Email);
         var errorResponse = new RestErrorResponse()
         {
            Title = "App Error",
            Status = HttpStatusCode.BadRequest,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
            TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
         };
         errorResponse.Errors["Email"] = new List<string>()
         {
            "Email already registered!"
         };
         return BadRequest(errorResponse);
         
      }

      var refreshToken = new RefreshToken();

      appUser = new AppUser()
      {
         Id = new Guid(),
         FirstName = adminRegister.FirstName,
         LastName = adminRegister.LastName,
         Gender = adminRegister.Gender,
         DateOfBirth = DateTime.Parse(adminRegister.DateOfBirth).ToUniversalTime(),
         PhoneNumber = adminRegister.PhoneNumber,
         Email = adminRegister.Email,
         UserName = adminRegister.Email,
         RefreshTokens = new List<RefreshToken>()
         {
            refreshToken
         }
      };

      
      var result = await _userManager.CreateAsync(appUser, adminRegister.Password);
      if (!result.Succeeded)
      {
         return BadRequest(result);
      }

#warning ask if this is the right way to add a claim in my app context
      result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname", 
         appUser.FirstName));
      result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname",
         appUser.LastName));

      appUser = await _userManager.FindByEmailAsync(appUser.Email);
      if (appUser == null)
      {
         
         _logger.LogWarning("User with email {} is not found after registration", adminRegister.Email);
         return BadRequest($"User with email {adminRegister.Email} is not found after registration");

      }
      #warning should it be a method
      var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);

      var jwt = IdentityExtension.GenerateJwt(
         claimsPrincipal.Claims,
         key: _configuration["JWT:Key"],
         issuer: _configuration["JWT:Issuer"],
         audience: _configuration["JWT:Issuer"],
         expirationDateTime: DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")));

      await _userManager.AddToRoleAsync(appUser, "Admin");
      
      var admin = new Admin
      {
         Id = new Guid(),
         AppUserId = appUser.Id,
         PersonalIdentifier = adminRegister.PersonalIdentifier,
         CityId = adminRegister.CityId,
         Address = adminRegister.Address,
         CreatedAt = DateTime.Now.ToUniversalTime(),
         UpdatedAt = DateTime.Now.ToUniversalTime()

      };
      _context.Admins.Add(admin);
      await _context.SaveChangesAsync();

      var adminDto = new AdminDTO()
      {
         Id = admin.Id,
         AppUserId = admin.AppUserId,
         Address = admin.Address,
         PersonalIdentifier = admin.PersonalIdentifier,
         CityId = admin.CityId,
         CreatedAt = admin.CreatedAt,
         CreatedBy = admin.CreatedBy,
         UpdatedAt = admin.UpdatedAt,
         UpdatedBy = admin.UpdatedBy
      };

      var res = new JwtResponseAdminRegister()
      {
         Token = jwt,
         AdminDTO = adminDto,
         RefreshToken = refreshToken.Token
         
      };
      return Ok(res);
   }
}