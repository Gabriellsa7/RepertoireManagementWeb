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

namespace RepertoireManagementWeb.Pages.RepertoirePages
{
    public class EditModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public EditModel(RepertoireManagementWeb.Data.AppDbContext context)
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

            var repertoire =  await _context.Repertoires.FirstOrDefaultAsync(m => m.Id == id);
            if (repertoire == null)
            {
                return NotFound();
            }
            Repertoire = repertoire;
           ViewData["BandId"] = new SelectList(_context.Bands, "Id", "Name");
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

            _context.Attach(Repertoire).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepertoireExists(Repertoire.Id))
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

        private bool RepertoireExists(Guid id)
        {
            return _context.Repertoires.Any(e => e.Id == id);
        }
    }
}
