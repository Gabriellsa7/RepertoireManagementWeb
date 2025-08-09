using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.UserPages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _context;

        public EditModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public new User User { get; set; }

        [BindProperty]
        public IFormFile ProfileImage { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if (User == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == User.Id);

            if (existingUser == null)
                return NotFound();

            existingUser.Name = User.Name;
            existingUser.Email = User.Email;
            existingUser.Password = User.Password;

            if (ProfileImage != null)
            {
                var uploadsFolder = Path.Combine("wwwroot", "uploads");
                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(ProfileImage.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await ProfileImage.CopyToAsync(fileStream);
                }

                existingUser.ImageUrl = "/uploads/" + uniqueFileName;
            }

            await _context.SaveChangesAsync();

            return RedirectToPage("/Profile");
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
