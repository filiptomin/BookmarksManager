using System;
using System.Collections.Generic;
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

namespace TestBookmarksDatabase.Administration.Roles
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private RoleManager<IdentityRole<Guid>> _roleManager;

        public List<IdentityRole<Guid>> RoleList { get; set; }
        //public List<SelectListItem> UserList { get; set; }
        private UserManager<IdentityUser<Guid>> _userManager;
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }

        public CreateModel(RoleManager<IdentityRole<Guid>> roleManager, UserManager<IdentityUser<Guid>> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public IActionResult OnGet()
        {
            RoleList = _roleManager.Roles.ToList();
            //UserList = _userManager.Users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName }).OrderBy(u => u.Text).ToList();
            //Bookmark = new Bookmark { Url = "https://" };
            return Page();
        }

        [BindProperty]
        public IdentityRole<Guid> Role { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var bm = await _roleManager.CreateAsync(Role);
                SuccessMessage = "Role has been added.";
                return RedirectToPage("./Index");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ErrorMessage = "Adding of role has failed.";
            }                

            return Page();
        }
    }
}
