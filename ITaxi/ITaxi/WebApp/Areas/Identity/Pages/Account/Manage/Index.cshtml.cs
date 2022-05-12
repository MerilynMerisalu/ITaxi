// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable enable

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using App.DAL.EF;
using App.Domain;
using App.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApp.Models.Enum;

namespace WebApp.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public IndexModel(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager, AppDbContext context,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string? StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; } = default!;

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; } = default!;

            [StringLength(50, MinimumLength = 1)]
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; } = default!;

            [StringLength(50, MinimumLength = 1)]
            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; } = default!;


            [EnumDataType(typeof(Gender))]
            [DisplayName(nameof(Models.Enum.Gender))]
            public Gender Gender { get; set; }

            [DataType(DataType.Date)]
            [DisplayName("Date of Birth")]
            public DateTime DateOfBirth { get; set; }


            [Display(Name = "Upload Image")] public IFormFile? ImageFile { get; set; }


        }

        private async Task LoadAsync(AppUser user)
        {

            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var firstname = user.FirstName;
            var lastName = user.LastName;
            var gender = user.Gender;
            var dateOfBirth = user.DateOfBirth.Date;
            



#warning Improve the profile accordingly to every type of user
            /*if (User.IsInRole(nameof(Admin)) || User.IsInRole(nameof(Driver)))
            {
                
                if (expr)
                {
                    
                }
            }*/

            /*if (User.IsInRole(nameof(Customer)))
            {
                
            }*/

#warning Add a profile picture uploading feature

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = firstname,
                LastName = lastName,
                Gender = gender,
                DateOfBirth = dateOfBirth,
                ImageFile = user.ProfileImage


            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }


            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }

            if (Input.FirstName != user.FirstName)
            {
                user.FirstName = Input.FirstName;
            }

            if (Input.LastName != user.LastName)
            {
                user.LastName = Input.LastName;
            }

            if (Input.Gender != user.Gender)
            {
                user.Gender = Input.Gender;
            }

            

            await CreatingAnImageToADisk(user.ProfileImage!);





            




            await _context.SaveChangesAsync();
            _context.Users.Update(user);

            await _signInManager.RefreshSignInAsync(user);

            StatusMessage = "Your profile has been updated";

            return RedirectToPage();
        }
        
        #warning continue when you have implementing the profile  the userId

        public async Task<IActionResult> CreatingAnImageToADisk(IFormFile imageFile)
        {
            var photo = new Photo();
            var user = await _userManager.GetUserAsync(User);
            
            var wwwRootPath = _webHostEnvironment.WebRootPath;
            if (Input.ImageFile!.Length > 0)
            {
                var fileName = Path.GetFileNameWithoutExtension(Input.ImageFile.FileName);
                var fileExtension = Path.GetExtension(Input.ImageFile.FileName);
                photo.PhotoName = $"{fileName}{DateTime.Now:g}{fileExtension}";
                photo.Title = fileName;
                var path = Path.Combine(wwwRootPath + "/Images/", fileName);

                  using (var fileStream = new FileStream(path, FileMode.Create))
                  {
                      await photo.ImageFile!.CopyToAsync(fileStream);
                  }

                  photo.AppUserId = user.Id;
                  await _context.Photos.AddAsync(photo);
                  await _context.SaveChangesAsync();
            }

            return RedirectToPage(nameof(Manage));
        }
    }
}

