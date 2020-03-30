using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration.Users
{
    public class DetailsModel : PageModel
    {
        private UserManager<IdentityUser<Guid>> _userManager;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public DetailsModel(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IdentityUser<Guid> IdentityUser { get; set; }
        public IdentityRole<Guid> IdentityRole { get; set; }

        public IActionResult OnGet(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }
            IdentityUser = _userManager.FindByIdAsync(id.ToString()).Result;
            List<string> Roles = _userManager.GetRolesAsync(_userManager.FindByIdAsync(id.ToString()).Result).Result.ToList();
            IdentityRole = _roleManager.FindByNameAsync(Roles[0].ToString()).Result;
            //IdentityRole = await _roleManager.FindByNameAsync(RoleList[0]);

            return Page();
        }
    }
}
