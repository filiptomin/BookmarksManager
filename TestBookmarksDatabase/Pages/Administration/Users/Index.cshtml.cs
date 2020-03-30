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
using Microsoft.EntityFrameworkCore;
using TestBookmarksDatabase.Models;
using TestBookmarksDatabase.Services;
using TestBookmarksDatabase.ViewModels;

namespace TestBookmarksDatabase.Administration.Users
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private UserManager<IdentityUser<Guid>> _userManager;
        [BindProperty(SupportsGet = true)]
        public string SearchFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid RoleFilter { get; set; }
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SuccessMessage { get; set; }
        [TempData]
        public string InfoMessage { get; set; }
        public IndexModel(UserManager<IdentityUser<Guid>> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public IList<BookmarksListViewModel> Bookmarks { get; set; }
        public List<IdentityUser<Guid>> Users { get; set; }

        public void OnGet()
        {
            //RoleList = _roleManager.Roles.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.Name }).OrderBy(u => u.Text).ToList();
            var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            Users = _userManager.Users.ToList();
            if (SearchFilter != null)
            {
                Users = _userManager.Users.Where(x => x.UserName.Contains(SearchFilter) || x.Email.Contains(SearchFilter)).ToList();
            }
            //if (RoleFilter != Guid.Empty)
            //{
            //    var Role = _roleManager.FindByIdAsync(RoleFilter.ToString()).Result;
            //    foreach (var x in _userManager.Users)
            //    {
            //        if (_userManager.GetRolesAsync(x).Result.Contains(Role.Name))
            //        {

            //        }
            //    }
            //    Users = _userManager.Users.Where(x => _userManager.GetRolesAsync(x).Result.Contains(_roleManager.FindByIdAsync(RoleFilter.ToString()).Result.Name)).ToList();
            //}

        }
    }
}
