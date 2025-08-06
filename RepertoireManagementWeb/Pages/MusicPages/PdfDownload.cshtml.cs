using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.MusicPages
{
    public class PdfDownloadModel : PageModel
    {
        private readonly AppDbContext _context;

        public PdfDownloadModel(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var music = await _context.Musics
                .FirstOrDefaultAsync(m => m.Id == id);

            if (music == null || music.PdfFile == null)
            {
                return NotFound();
            }

            // Not return to a view Razor: only send the file
            return File(music.PdfFile, "application/pdf");
        }
    }
}
