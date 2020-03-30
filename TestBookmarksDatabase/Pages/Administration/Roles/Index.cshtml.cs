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

namespace TestBookmarksDatabase.Administration.Roles
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private RoleManager<IdentityRole<Guid>> _roleManager;
        private UserManager<IdentityUser<Guid>> _userManager;
        [BindProperty(SupportsGet = true)]
        public string SearchFilter { get; set; }
        [BindProperty(SupportsGet = true)]
        public Guid OwnerFilter { get; set; }
        public List<SelectListItem> UserList { get; set; }
        public List<IdentityRole<Guid>> RoleList { get; set; }
        [BindProperty(SupportsGet = true)]
        public BookmarkListOrder Order { get; set; }
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

        public void OnGetAsync()
        {
            RoleList = _roleManager.Roles/*.Where(x => x.Name.Contains(SearchFilter) || x.NormalizedName.Contains(SearchFilter))*/.ToList();
            if(SearchFilter != null)
            {
                RoleList = _roleManager.Roles.Where(x => x.Name.Contains(SearchFilter) || x.NormalizedName.Contains(SearchFilter)).ToList();
            }
            //UserList = _userManager.Users.Select(u => new SelectListItem { Value = u.Id.ToString(), Text = u.UserName }).OrderBy(u => u.Text).ToList();
            //var currentUserId = Guid.Parse(User.Claims.Where(c => c.Type == ClaimTypes.NameIdentifier).FirstOrDefault().Value);
            //Bookmarks = _bookmarksManager.List(OwnerFilter, SearchFilter, null, Order).Result.ToList();
        }
    }
}
