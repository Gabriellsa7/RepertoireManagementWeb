using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.RepertoirePages
{
    public class DeleteModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public DeleteModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Repertoire Repertoire { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoire = await _context.Repertoires.FirstOrDefaultAsync(m => m.Id == id);

            if (repertoire == null)
            {
                return NotFound();
            }
            else
            {
                Repertoire = repertoire;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoire = await _context.Repertoires.FindAsync(id);
            if (repertoire != null)
            {
                Repertoire = repertoire;
                _context.Repertoires.Remove(Repertoire);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
