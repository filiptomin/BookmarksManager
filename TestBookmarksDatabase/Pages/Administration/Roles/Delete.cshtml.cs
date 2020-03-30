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

namespace TestBookmarksDatabase.Administration.Roles
{
    public class DeleteModel : PageModel
    {
        private RoleManager<IdentityRole<Guid>> _roleManager;
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }

        public DeleteModel(RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole<Guid> Role { get; set; }

        public IActionResult OnGet(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Role = _roleManager.FindByIdAsync(id.ToString()).Result;

            if (Role == null)
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

            Role = _roleManager.FindByIdAsync(id.ToString()).Result;

            if (Role != null)
            {
                try
                {
                    await _roleManager.DeleteAsync(Role);
                    SuccessMessage = "Role has been removed.";
                }
                catch
                {
                    ErrorMessage = "There was error during role removal.";
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
