using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RepertoireManagementWeb.Data;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RepertoireManagementWeb.Pages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private readonly AppDbContext _context;

        public ProfileModel(AppDbContext context)
        {
            _context = context;
        }

        public string UserName { get; set; }
        public string ImageUrl { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/Login");
            }

            var user = _context.Users.FirstOrDefault(u => u.Name == username);

            if (user == null)
            {
                return RedirectToPage("/Login");
            }

            UserName = user.Name;
            ImageUrl = user.ImageUrl;

            return Page();
        }

        public async Task<IActionResult> OnPostLogoutAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToPage("/Login");
        }
    }
}
