using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using App.DAL.DTO.AdminArea;
using App.DAL.EF;
using App.Domain;
using App.Domain.DTO;
using App.Domain.DTO.AdminArea;
using App.Domain.Identity;
using Base.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Areas.Identity.Pages.Account;
using WebApp.DTO;
using WebApp.DTO.Identity;
using DriverAndDriverLicenseCategoryDTO = WebApp.DTO.DriverAndDriverLicenseCategoryDTO;

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
    private readonly AppDbContext _context;

    public AccountController(SignInManager<AppUser> signInManager,
        UserManager<AppUser> userManager,
        IConfiguration configuration,
        ILogger<AccountController> logger, AppDbContext context)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _configuration = configuration;
        _logger = logger;
        _context = context;
    }

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
            FirstName = customerRegistrationDTO.FirstName,
            LastName = customerRegistrationDTO.LastName,
            Gender = customerRegistrationDTO.Gender,
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

        var jwt = IdentityExtension.GenerateJwt(
            claimsPrincipal.Claims,
            key: _configuration["JWT:Key"],
            issuer: _configuration["JWT:Issuer"],
            audience: _configuration["JWT:Issuer"],
            expirationDateTime: refreshToken.TokenExpirationDateAndTime
        );
        await _userManager.AddToRoleAsync(appUser, "Customer");

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

    [HttpPost]
    public async Task<ActionResult<JwtResponseAdminRegister>> RegisterAdminDTO(AdminRegistrationDTO adminRegistrationDTO)
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
            FirstName = adminRegistrationDTO.FirstName,
            LastName = adminRegistrationDTO.LastName,
            Gender = adminRegistrationDTO.Gender,
            DateOfBirth = DateTime.Parse(adminRegistrationDTO.DateOfBirth).ToUniversalTime(),
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
      
        var admin = new Admin
        {
            Id = new Guid(),
            AppUserId = appUser.Id,
            PersonalIdentifier = adminRegistrationDTO.PersonalIdentifier,
            CityId = adminRegistrationDTO.CityId,
            Address = adminRegistrationDTO.Address,
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
            AdminDto = adminDto,
            RefreshToken = refreshToken.Token,
        };
            
        return Ok(res);
    }

    [HttpPost]
    public async Task<ActionResult<JwtResponseAdminRegister>> RegisterDriverDTO(DriverRegistrationDTO driverRegistrationDto)
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
            FirstName = driverRegistrationDto.FirstName,
            LastName = driverRegistrationDto.LastName,
            Gender = driverRegistrationDto.Gender,
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

        var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(appUser);
        if (claimsPrincipal == null)
        {
            _logger.LogWarning("Could not get claims for user {}!", driverRegistrationDto.Email);
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
        await _userManager.AddToRoleAsync(appUser, "Driver");
      
        var driver = new Driver()
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
        _context.Drivers.Add(driver);
        await _context.SaveChangesAsync();

        
        if (driverRegistrationDto.DriverLicenseCategories != null)
            foreach (var driverLicenseCategoryId in driverRegistrationDto.DriverLicenseCategories)
            {
                var driverAndDriverLicenseCategory = new DriverAndDriverLicenseCategory()
                {
                    Id = new Guid(),
                    DriverLicenseCategoryId = driverLicenseCategoryId,
                    DriverId = driver.Id,
                };

                await _context.AddAsync(driverAndDriverLicenseCategory);
            }

        await _context.SaveChangesAsync();

        var driverLicenseCategoryNames = await _context.DriverLicenseCategories
            .Include(dl =>
                dl.Drivers!.Where(d => d.DriverId.Equals(driver.Id)))
            .Select(dl => dl.DriverLicenseCategoryName).ToListAsync();

        var driverDto = new DriverDTO()
        {
            Id = driver.Id,
            AppUserId = driver.AppUserId,
            Address = driver.Address,
            PersonalIdentifier = driver.PersonalIdentifier,
            CityId = driver.CityId,
            NumberOfDriverLicenseCategories = driverRegistrationDto.DriverLicenseCategories!.Count,
            
            DriverLicenseNumber = driver.DriverLicenseNumber,
            DriverLicenseExpiryDate = driver.DriverLicenseExpiryDate.ToLocalTime().ToShortDateString(),
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
            DriverAndDriverLicenseCategoryDTO = new DriverAndDriverLicenseCategoryDTO()
            {
                DriverLicenseCategoryNames = driverLicenseCategoryNames
            }
            
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
        
        await _context.SaveChangesAsync();
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

        await _context.Entry(appUser).Collection(u => u.RefreshTokens!)
            .Query().Where(x => (x.Token == refreshTokenModel.RefreshToken && 
                                 x.TokenExpirationDateAndTime > DateTime.UtcNow) ||
                                x.PreviousToken == refreshTokenModel.RefreshToken &&
                                x.PreviousTokenExpirationDateAndTime > DateTime.UtcNow).ToListAsync();
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
}