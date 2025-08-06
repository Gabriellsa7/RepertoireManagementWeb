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

namespace RepertoireManagementWeb.Pages.MusicPages
{
    public class EditModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public EditModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Music Music { get; set; } = default!;

        [BindProperty]
        public IFormFile? PdfUpload { get; set; }


        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var music =  await _context.Musics.FirstOrDefaultAsync(m => m.Id == id);
            if (music == null)
            {
                return NotFound();
            }
            Music = music;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var musicInDb = await _context.Musics.FirstOrDefaultAsync(m => m.Id == Music.Id);
            if (musicInDb == null)
            {
                return NotFound();
            }

            // Update variables
            musicInDb.Title = Music.Title;
            musicInDb.ImageUrl = Music.ImageUrl;

            // If sending a new PDF, update.
            if (PdfUpload != null && PdfUpload.Length > 0)
            {
                using var memoryStream = new MemoryStream();
                await PdfUpload.CopyToAsync(memoryStream);
                musicInDb.PdfFile = memoryStream.ToArray();
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MusicExists(Music.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }


        private bool MusicExists(Guid id)
        {
            return _context.Musics.Any(e => e.Id == id);
        }
    }
}
