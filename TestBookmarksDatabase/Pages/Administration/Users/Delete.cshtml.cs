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
    public class DeleteModel : PageModel
    {
        private UserManager<IdentityUser<Guid>> _userManager;
        //private AspNetRoleManager<IdentityUserRole<Guid>> _netRoleManager;
        private ApplicationDbContext Db;
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }

        public DeleteModel(UserManager<IdentityUser<Guid>> userManager, AspNetRoleManager<IdentityUserRole<Guid>> netRoleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            //_netRoleManager = netRoleManager;
            Db = db;
        }

        [BindProperty]
        public IdentityUser<Guid> IdentityUser { get; set; }

        public IActionResult OnGet(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser = _userManager.FindByIdAsync(id.ToString()).Result;

            if (IdentityUser == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdentityUser = _userManager.FindByIdAsync(id.ToString()).Result;

            if (IdentityUser != null)
            {
                try
                {
                    await _userManager.DeleteAsync(_userManager.FindByIdAsync(id.ToString()).Result);
                    await Db.SaveChangesAsync();
                    SuccessMessage = "User has been removed.";
                }
                catch
                {
                    ErrorMessage = "There was error during user removal.";
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
