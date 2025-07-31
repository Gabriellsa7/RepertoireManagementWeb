using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.BandPages.RepertoirePage
{
    public class AddRepertoireModel : PageModel
    {
        private readonly AppDbContext _context;

        public AddRepertoireModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Guid BandId { get; set; }

        [BindProperty]
        public Guid SelectedRepertoireId { get; set; }

        public Band Band { get; set; } = null!;
        public SelectList AvailableRepertoires { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {
            Band = await _context.Bands.FindAsync(BandId);
            if (Band == null)
                return NotFound();

            var repertoires = await _context.Repertoires
                .Where(r => r.BandId == null)
                .ToListAsync();

            AvailableRepertoires = new SelectList(repertoires, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var band = await _context.Bands.FindAsync(BandId);
            var repertoire = await _context.Repertoires.FindAsync(SelectedRepertoireId);

            if (band == null || repertoire == null)
                return NotFound();

            // Associa o repertório à banda
            repertoire.BandId = BandId;

            await _context.SaveChangesAsync();

            return RedirectToPage("/BandPages/Details", new { id = BandId });
        }
    }
}
