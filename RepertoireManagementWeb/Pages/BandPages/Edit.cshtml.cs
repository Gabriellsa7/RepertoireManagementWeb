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

namespace RepertoireManagementWeb.Pages.BandPages
{
    public class EditModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public EditModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Band Band { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band =  await _context.Bands.FirstOrDefaultAsync(m => m.Id == id);
            if (band == null)
            {
                return NotFound();
            }
            Band = band;
           ViewData["LeaderId"] = new SelectList(_context.Users, "Id", "Email");
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

            _context.Attach(Band).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BandExists(Band.Id))
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

        private bool BandExists(Guid id)
        {
            return _context.Bands.Any(e => e.Id == id);
        }
    }
}
