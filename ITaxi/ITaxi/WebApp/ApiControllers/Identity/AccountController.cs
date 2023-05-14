using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.BLL.DTO.AdminArea;
using App.Contracts.BLL;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using App.Enum.Enum;
using App.Public.DTO.v1.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.DTO;
using WebApp.DTO.Identity;
using AppUser = App.Domain.Identity.AppUser;


namespace WebApp.ApiControllers.Identity;
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/identity/[controller]/[action]")]
[ApiController]

public class AccountController : ControllerBase
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AccountController> _logger;
    private readonly Random _rand = new Random();
    #warning code smell check it
    [Obsolete("Move this code to AppBll", false)]
    private readonly AppDbContext _context;
    #warning code smell check it
    private readonly IAppBLL _appBLL;

    
    public AccountController(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        ILogger<AccountController> logger, AppDbContext context, IAppBLL appBLL
        )
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
        _context = context;
        _appBLL = appBLL;
    }

    /// <summary>
    /// Customer registration api endpoint
    /// </summary>
    /// <param name="customerRegistrationDTO">Customer registration DTO which holds data for the registration</param>
    /// <returns>JwtResponseCustomerRegister</returns>
    [HttpPost]
    public async Task<ActionResult<JwtResponseCustomerRegister>> RegisterCustomerDTO
        (CustomerRegistrationDTO customerRegistrationDTO)
    {
         var appUser = await _userManager.FindByEmailAsync(customerRegistrationDTO.Email);
        if (appUser != null)
        {
            _logger.LogWarning("Webapi user registration failed! User with an email {} already exist!",
                customerRegistrationDTO.Email);
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Email"] = new List<string>()
                    {
                        "Email already registered!"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var refreshToken = new RefreshToken
        {
            TokenExpirationDateAndTime = DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")).ToUniversalTime()
        };
        appUser = new AppUser()
        {
            Id = Guid.NewGuid(),
            FirstName = customerRegistrationDTO.FirstName,
            LastName = customerRegistrationDTO.LastName,
            Gender = Enum.Parse<Gender>(customerRegistrationDTO.Gender.ToString()),
            DateOfBirth = DateTime.Parse(customerRegistrationDTO.DateOfBirth).ToUniversalTime(),
            PhoneNumber = customerRegistrationDTO.PhoneNumber,
            Email = customerRegistrationDTO.Email,
            UserName = customerRegistrationDTO.Email,
            EmailConfirmed = true,
            RefreshTokens = new Collection<RefreshToken>()
            {
                refreshToken
            }

        };

        var result = await _userManager.CreateAsync(appUser, customerRegistrationDTO.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname",
            appUser.FirstName));
        
        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));
        appUser = await _userManager.FindByEmailAsync(appUser.Email);

        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", customerRegistrationDTO.Email);
            return BadRequest($"User with email {customerRegistrationDTO.Email} is not found after registration");

        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claims for user {}!", customerRegistrationDTO.Email);
            await Task.Delay(_rand.Next(100, 1000));
            return NotFound("Username / password problem!");
        }
        await _userManager.AddToRoleAsync(appUser, "Customer");

        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            key: _configuration["JWT:Key"],
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Issuer"],
            expirationDateTime: refreshToken.TokenExpirationDateAndTime
        );
        

        
        
        var customer = new Customer()
        {
            AppUserId = appUser.Id,
            DisabilityTypeId = customerRegistrationDTO.DisabilityTypeId,
            CreatedBy = User.Identity!.Name! ?? "",
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedBy = User.Identity!.Name ?? ""
        };
         
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        var customerDto = new CustomerDTO()
        {
            Id = customer.Id,
            DisabilityTypeId = customerRegistrationDTO.DisabilityTypeId
        };
        
        var res = new JwtResponseCustomerRegister()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            CustomerDTO = customerDto
        };
            
        return Ok(res);
    }

    /// <summary>
    /// Admin registration api endpoint
    /// </summary>
    /// <param name="adminRegistrationDTO">Admin registration DTO which holds data for the registration</param>
    /// <returns>JwtResponseAdminRegister</returns>
    [HttpPost]
    public async Task<ActionResult<JwtResponseAdminRegister>> RegisterAdminDTO(AdminRegistration adminRegistrationDTO)
    {
        var appUser = await _userManager.FindByEmailAsync(adminRegistrationDTO.Email);
        if (appUser != null)
        {
            _logger.LogWarning("Webapi user registration failed! User with an email {} already exist!",
                adminRegistrationDTO.Email);
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Email"] = new List<string>()
                    {
                        "Email already registered!"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var refreshToken = new RefreshToken
        {
            TokenExpirationDateAndTime = DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")).ToUniversalTime()
        };
        
        appUser = new AppUser()
        {
            Id = Guid.NewGuid(),
            FirstName = adminRegistrationDTO.FirstName,
            LastName = adminRegistrationDTO.LastName,
            Gender = Enum.Parse<Gender>(adminRegistrationDTO.Gender.ToString()),
            DateOfBirth = adminRegistrationDTO.DateOfBirth.ToUniversalTime(),
            PhoneNumber = adminRegistrationDTO.PhoneNumber,
            Email = adminRegistrationDTO.Email,
            UserName = adminRegistrationDTO.Email,
            RefreshTokens = new Collection<RefreshToken>()
            {
                refreshToken
            }

        };

        var result = await _userManager.CreateAsync(appUser, adminRegistrationDTO.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname",
            appUser.FirstName));
        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));

        appUser = await _userManager.FindByEmailAsync(appUser.Email);

        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", adminRegistrationDTO.Email);
            return BadRequest($"User with email {adminRegistrationDTO.Email} is not found after registration");

        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claims for user {}!", adminRegistrationDTO.Email);
            await Task.Delay(_rand.Next(100, 1000));
            return NotFound("Username / password problem!");
        }

        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            key: _configuration["JWT:Key"],
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Issuer"],
            expirationDateTime: refreshToken.TokenExpirationDateAndTime
        );
        await _userManager.AddToRoleAsync(appUser, "Admin");
      
        var admin = new App.BLL.DTO.AdminArea.AdminDTO()
        {
            Id = new Guid(),
            AppUserId = appUser.Id,
            PersonalIdentifier = adminRegistrationDTO.PersonalIdentifier,
            CityId = adminRegistrationDTO.CityId,
            Address = adminRegistrationDTO.Address,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
        _appBLL.Admins.Add(admin);
        await _appBLL.SaveChangesAsync();

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
            AdminDto = adminDto,
            RefreshToken = refreshToken.Token,
        };
            
        return Ok(res);
    }
/// <summary>
/// Admin registration api endpoint
/// </summary>
/// <param name="driverRegistrationDto">Admin registration DTO which holds data for the registration</param>
/// <returns>JwtResponseAdminRegister</returns>
    [HttpPost]
    public async Task<ActionResult<JwtResponseDriverRegister>> RegisterDriverDTO(DriverRegistrationDTO driverRegistrationDto)
    {
        var appUser = await _userManager.FindByEmailAsync(driverRegistrationDto.Email);
        if (appUser != null)
        {
            _logger.LogWarning("Webapi user registration failed! User with an email {} already exist!",
                driverRegistrationDto.Email);
            var errorResponse = new RestErrorResponse
            {
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1",
                Title = "App error",
                Status = HttpStatusCode.BadRequest,
                TraceId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                Errors =
                {
                    ["Email"] = new List<string>()
                    {
                        "Email already registered!"
                    }
                }
            };
            return BadRequest(errorResponse);
        }

        var refreshToken = new RefreshToken
        {
            TokenExpirationDateAndTime = DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")).ToUniversalTime()
        };
        appUser = new AppUser()
        {
            Id = Guid.NewGuid(),
            FirstName = driverRegistrationDto.FirstName,
            LastName = driverRegistrationDto.LastName,
            Gender = Enum.Parse<Gender>(driverRegistrationDto.Gender.ToString()),
            DateOfBirth = DateTime.Parse(driverRegistrationDto.DateOfBirth).ToUniversalTime(),
            PhoneNumber = driverRegistrationDto.PhoneNumber,
            Email = driverRegistrationDto.Email,
            UserName = driverRegistrationDto.Email,
            EmailConfirmed = true,
            RefreshTokens = new Collection<RefreshToken>()
            {
                refreshToken
            }

        };

        var result = await _userManager.CreateAsync(appUser, driverRegistrationDto.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result);
        }

        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.firstname",
            appUser.FirstName));
        result = await _userManager.AddClaimAsync(appUser, new Claim("aspnet.lastname", appUser.LastName));

        appUser = await _userManager.FindByEmailAsync(appUser.Email);

        if (appUser == null)
        {
            _logger.LogWarning("User with email {} is not found after registration", driverRegistrationDto.Email);
            return BadRequest($"User with email {driverRegistrationDto.Email} is not found after registration");

        }
        await _userManager.AddToRoleAsync(appUser, "Driver");

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claims for user {}!", driverRegistrationDto.Email);
            await Task.Delay(_rand.Next(100, 1000));
            return NotFound("Username / password problem!");
        }
        await _userManager.AddToRoleAsync(appUser, "Driver");

        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            key: _configuration["JWT:Key"],
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Issuer"],
            expirationDateTime: refreshToken.TokenExpirationDateAndTime
        );
        
      
        var driver = new App.BLL.DTO.AdminArea.DriverDTO()
        {
            Id = new Guid(),
            AppUserId = appUser.Id,
            PersonalIdentifier = driverRegistrationDto.PersonalIdentifier,
            CityId = driverRegistrationDto.CityId,
            Address = driverRegistrationDto.Address,
            DriverLicenseNumber = driverRegistrationDto.DriverLicenseNumber,
            DriverLicenseExpiryDate = DateTime.Parse(driverRegistrationDto.DriverLicenseExpiryDate)
                .ToUniversalTime(),
            CreatedAt = DateTime.Now.ToUniversalTime(),
            UpdatedAt = DateTime.Now.ToUniversalTime()
        };
        _appBLL.Drivers.Add(driver);
        await _appBLL.SaveChangesAsync();

        
        if (driverRegistrationDto.DriverLicenseCategories != null)
            foreach (var driverLicenseCategoryId in driverRegistrationDto.DriverLicenseCategories)
            {
                var driverAndDriverLicenseCategory = new App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO()
                {
                    Id = new Guid(),
                    DriverLicenseCategoryId = driverLicenseCategoryId,
                    DriverId = driver.Id,
                };

                 _appBLL.DriverAndDriverLicenseCategories.Add(driverAndDriverLicenseCategory);
            }

        await _appBLL.SaveChangesAsync();

        var driverLicenseCategoryNames =
            await _appBLL.DriverAndDriverLicenseCategories
                .GetAllDriverLicenseCategoriesBelongingToTheDriverAsync(driver.Id);
            

        var driverDto = new App.BLL.DTO.AdminArea.DriverDTO()
        {
            Id = driver.Id,
            AppUserId = driver.AppUserId,
            Address = driver.Address,
            PersonalIdentifier = driver.PersonalIdentifier,
            CityId = driver.CityId,
            NumberOfDriverLicenseCategories = driverRegistrationDto.DriverLicenseCategories!.Count,
            
            DriverLicenseNumber = driver.DriverLicenseNumber,
            DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate,
            CreatedAt = driver.CreatedAt,
            CreatedBy = driver.CreatedBy,
            UpdatedAt = driver.UpdatedAt,
            UpdatedBy = driver.UpdatedBy
        };
        
        var res = new JwtResponseDriverRegister()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            DriverDTO = driverDto,
            DriverAndDriverLicenseCategoryDTO = new App.BLL.DTO.AdminArea.DriverAndDriverLicenseCategoryDTO()
            {
                DriverLicenseCategoryNames = driverLicenseCategoryNames
            }
            
        };
            
        return Ok(res);
    }
/// <summary>
/// Log in api endpoint
/// </summary>
/// <param name="loginData">Supply email and password</param>
/// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<JwtResponse>> LogIn([FromBody] LoginDTO loginData)
    {
        var appUser = await _userManager.FindByEmailAsync(loginData.Email);
        if (appUser == null)
        {
            _logger.LogWarning("Webapi login failed! Email {} not found!", loginData.Email);
            await Task.Delay(_rand.Next(100, 1000));
            return NotFound("Username / password problem");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(appUser, loginData.Password,
            false);
        if (!result.Succeeded)
        {
            _logger.LogWarning("Webapi login failed! Password problem for user {}", loginData.Email);
            await Task.Delay(_rand.Next(100, 1000));
            return NotFound("Username / password problem");
        }

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claims for user {}!", loginData.Email);
            await Task.Delay(_rand.Next(100, 1000));
            return NotFound("Username / password problem!");
        }

        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            key: _configuration["JWT:Key"],
            issuer:_configuration["JWT:Issuer"],
            audience: _configuration["JWT:Issuer"],
            expirationDateTime: DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes")));
        
        
        var refreshToken = new RefreshToken
        {
            TokenExpirationDateAndTime = DateTime.Now.AddMinutes(_configuration.GetValue<int>("JWT:ExpireInMinutes"))
                .ToUniversalTime()
            
        };
        await _context.Entry(appUser)
            .Collection(a => a.RefreshTokens!).Query().ToListAsync();
        appUser.RefreshTokens!.Add(refreshToken);

        var roles = await _userManager.GetRolesAsync(appUser);
        await _context.SaveChangesAsync();
        var res = new JwtResponse()
        {
            Token = jwt,
            RefreshToken = refreshToken.Token,
            FirstName = appUser.FirstName,
            LastName = appUser.LastName,
            RoleNames = roles.ToArray()
        };
        return Ok(res);
    }
/// <summary>
/// Generating a refresh token to grant access without reentering credentials
/// </summary>
/// <param name="refreshTokenModel">Refresh token model DTO which holds data for the refresh token</param>
/// <returns>Action result OK</returns>
    [HttpPost]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModelDTO refreshTokenModel)
    {
        JwtSecurityToken jwtToken;
        try
        {
             jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(refreshTokenModel.Token);
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

        var userEmail = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
        if (userEmail == null)
        {
            return BadRequest("No email in jwt");
        }

        var appUser = await _userManager.FindByEmailAsync(userEmail);
        if (appUser == null)
        {
            return BadRequest($"User with email ${userEmail} was not found!");
        }

        List<RefreshToken> tokens = await GetRefreshTokens(appUser, refreshTokenModel.Token);
   
        if (appUser.RefreshTokens == null)
        {
            return Problem("Refresh token collection is null");

        }

        if (appUser.RefreshTokens!.Count == 0)
        {
            return Problem("Refresh token collection is empty! No valid refresh tokens found!");
        }
        if (appUser.RefreshTokens!.Count != 1)
        {
            return Problem("More than one valid token found");
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

        if (refreshToken.Token == refreshTokenModel.RefreshToken)
        {
            refreshToken.PreviousToken = refreshToken.Token;
            refreshToken.PreviousTokenExpirationDateAndTime = DateTime.UtcNow.AddMinutes(1);

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

#pragma warning disable 612, 618 
    protected async Task<List<RefreshToken>> GetRefreshTokens(AppUser appUser, string token)
    {
        return await _context.Entry(appUser).Collection(u => u.RefreshTokens!)                               
            .Query().Where(x => (x.Token == token &&                         
                                 x.TokenExpirationDateAndTime > DateTime.UtcNow) ||                   
                                x.PreviousToken == token &&                  
                                x.PreviousTokenExpirationDateAndTime > DateTime.UtcNow).ToListAsync();
    }
}