using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.RepertoireMusicPages
{
    public class DeleteModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public DeleteModel(RepertoireManagementWeb.Data.AppDbContext context)
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

            var repertoiremusic = await _context.RepertoireMusics.FirstOrDefaultAsync(m => m.Id == id);

            if (repertoiremusic == null)
            {
                return NotFound();
            }
            else
            {
                RepertoireMusic = repertoiremusic;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoiremusic = await _context.RepertoireMusics.FindAsync(id);
            if (repertoiremusic != null)
            {
                RepertoireMusic = repertoiremusic;
                _context.RepertoireMusics.Remove(RepertoireMusic);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
