using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration.Users
{
    public class CreateModel : PageModel
    {
        public List<SelectListItem> RoleList { get; set; }
        private UserManager<IdentityUser<Guid>> _userManager;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        //private AspNetRoleManager<IdentityUserRole<Guid>> _netRoleManager;
        private readonly ApplicationDbContext Db;

        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }

        public CreateModel( UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager, AspNetRoleManager<IdentityUserRole<Guid>> netRoleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            //_netRoleManager = netRoleManager;
            Db = db;
        }


        public IActionResult OnGet()
        {
            RoleList = _roleManager.Roles.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).OrderBy(u => u.Text).ToList();
            return Page();
        }
        [BindProperty]
        public RegisterInputModel Input { get; set; }
        [BindProperty]
        public IdentityUser<Guid> NewUser { get; set; }
        [BindProperty]
        public IdentityUserRole<Guid> Connect { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var hasher = new PasswordHasher<IdentityUser>();
            try
            {
                await _userManager.CreateAsync(NewUser = new IdentityUser<Guid>
                {
                    Id = Guid.NewGuid(),
                    NormalizedEmail = NewUser.Email.ToUpper(),
                    EmailConfirmed = false,
                    LockoutEnabled = false,
                    NormalizedUserName = NewUser.UserName.ToUpper(),
                    PasswordHash = hasher.HashPassword(null, Input.Password),
                    SecurityStamp = string.Empty,
                });
                await _userManager.AddToRoleAsync(_userManager.FindByIdAsync(NewUser.Id.ToString()).Result, _roleManager.FindByIdAsync(Connect.RoleId.ToString()).Result.Name);
                //await _userManager.CreateAsync(new IdentityUserRole<Guid> { RoleId = Guid.Parse(Connect.RoleId.ToString()), UserId = Guid.Parse(NewUser.Id.ToString()) });
                await Db.SaveChangesAsync();
                SuccessMessage = "User has been added.";
                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ErrorMessage = "Creating of user has failed.";
            }
            return Page();
        }
    }
    public class RegisterInputModel
    {
        //[Required]
        //[Display(Name = "Jméno")]
        //public string FirstName { get; set; }
        //[Display(Name = "Prostřední jméno")]
        //public string MiddleName { get; set; }
        //[Required]
        //public string LastName { get; set; }
        //[Required]
        //[Display(Name = "Uživatelské jméno")]
        //public string UserName { get; set; }
        //[Display(Name = "Telefonní číslo")]
        //public PhoneAttribute PhoneNumber { get; set; }
        //[Required]
        //public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Heslo musí mít délku mezi 6 a 100 znaky.", MinimumLength = 6)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Hesla se musí shodovat.")]
        public string ConfirmPassword { get; set; }
    }
}
