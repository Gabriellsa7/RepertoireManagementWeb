using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;
using System.IO;


namespace RepertoireManagementWeb.Pages.MusicPages
{
    public class CreateModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public CreateModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Music Music { get; set; } = default!;

        [BindProperty]
        public IFormFile? PdfUpload { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (PdfUpload != null && PdfUpload.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await PdfUpload.CopyToAsync(memoryStream);
                Music.PdfFile = memoryStream.ToArray();
            }

            _context.Musics.Add(Music);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
