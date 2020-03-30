using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;

namespace TestBookmarksDatabase.Administration.Roles
{
    public class EditModel : PageModel
    {
        private RoleManager<IdentityRole<Guid>> _roleManager;
        public List<SelectListItem> UserList { get; set; }
        private UserManager<IdentityUser<Guid>> _userManager;
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public EditModel(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager, ApplicationDbContext db)
        {
            Db = db;
            _roleManager = roleManager;
            _userManager = userManager;
        }
        private readonly ApplicationDbContext Db;
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            try
            {
                await _roleManager.SetRoleNameAsync(_roleManager.FindByIdAsync(Role.Id.ToString()).Result,Role.Name.ToString());
                await Db.SaveChangesAsync();
                SuccessMessage = "Role name was updated.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _roleManager.RoleExistsAsync(Role.Name))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch 
            {
                ErrorMessage = "Role was not updated.";
            }

            return RedirectToPage("./Index");
        }
    }
}
