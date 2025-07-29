using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.MusicPages
{
    public class DeleteModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public DeleteModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Music Music { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var music = await _context.Musics.FirstOrDefaultAsync(m => m.Id == id);

            if (music == null)
            {
                return NotFound();
            }
            else
            {
                Music = music;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var music = await _context.Musics.FindAsync(id);
            if (music != null)
            {
                Music = music;
                _context.Musics.Remove(Music);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
