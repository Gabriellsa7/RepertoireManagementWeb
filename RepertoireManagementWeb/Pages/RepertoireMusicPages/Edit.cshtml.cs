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

namespace RepertoireManagementWeb.Pages.RepertoireMusicPages
{
    public class EditModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public EditModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public RepertoireMusic RepertoireMusic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoiremusic =  await _context.RepertoireMusics.FirstOrDefaultAsync(m => m.Id == id);
            if (repertoiremusic == null)
            {
                return NotFound();
            }
            RepertoireMusic = repertoiremusic;
           ViewData["MusicId"] = new SelectList(_context.Musics, "Id", "Title");
           ViewData["RepertoireId"] = new SelectList(_context.Repertoires, "Id", "Description");
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

            _context.Attach(RepertoireMusic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepertoireMusicExists(RepertoireMusic.Id))
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

        private bool RepertoireMusicExists(Guid id)
        {
            return _context.RepertoireMusics.Any(e => e.Id == id);
        }
    }
}
