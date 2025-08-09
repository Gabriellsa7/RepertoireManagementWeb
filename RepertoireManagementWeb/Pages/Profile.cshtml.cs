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

        [BindProperty]
        public IFormFile ProfileImage { get; set; }
        public async Task<IActionResult> OnPostUploadImageAsync()
        {
            var username = User.Identity.Name;
            var user = _context.Users.FirstOrDefault(u => u.Name == username);

            if (user == null || ProfileImage == null)
                return Page();

            var uploadsFolder = Path.Combine("wwwroot", "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await ProfileImage.CopyToAsync(fileStream);
            }

            user.ImageUrl = "/uploads/" + uniqueFileName;
            _context.SaveChanges();

            return RedirectToPage(); 
        }

        public Guid UserId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var username = User.Identity.Name;

            if (string.IsNullOrEmpty(username))
                return RedirectToPage("/Login");

            var user = _context.Users.FirstOrDefault(u => u.Name == username);

            if (user == null)
                return RedirectToPage("/Login");

            UserId = user.Id;
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
