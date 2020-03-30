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

namespace TestBookmarksDatabase.Administration.Users
{
    public class EditModel : PageModel
    {
        public List<SelectListItem> UserList { get; set; }
        private UserManager<IdentityUser<Guid>> _userManager;
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private ApplicationDbContext Db;

        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public EditModel(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            Db = db;
        }
        [BindProperty]
        public IdentityUser<Guid> IdentityUser { get; set; }
        public List<SelectListItem> RoleList { get; set; }
        [BindProperty]
        public IdentityRole<Guid> IdentityRole { get; set; }

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
            RoleList = _roleManager.Roles.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).OrderBy(u => u.Text).ToList();

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
                await _userManager.SetUserNameAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityUser.UserName.ToString());
                //await _userManager.RemoveFromRoleAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, _roleManager.Roles.SingleOrDefault(x => x != IdentityRole).Name);
                //await _userManager.AddToRoleAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityRole.Name);
                await _userManager.ChangeEmailAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityUser.Email, _userManager.GenerateChangeEmailTokenAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityUser.Email).Result.ToString());
                await _userManager.ChangePhoneNumberAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityUser.PhoneNumber, _userManager.GenerateChangePhoneNumberTokenAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityUser.PhoneNumber).Result.ToString());
                //await _userManager.ChangePhoneNumberAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityUser.PhoneNumber, _userManager.GenerateChangePhoneNumberTokenAsync(_userManager.FindByIdAsync(IdentityUser.Id.ToString()).Result, IdentityUser.PhoneNumber).ToString());
                await Db.SaveChangesAsync();
                SuccessMessage = "User was edited.";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_userManager.Users.Any(x => x == IdentityUser))
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
                ErrorMessage = "User was not updated.";
            }

            return RedirectToPage("./Index");
        }
    }
}
