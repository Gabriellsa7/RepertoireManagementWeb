using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        [BindProperty]
        public Repertoire NewRepertoire { get; set; } = new Repertoire();

        [BindProperty(SupportsGet = true)]
        public Guid BandId { get; set; }

        public Band? Band { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Band = await _context.Bands.FindAsync(BandId);
            if (Band == null)
                return NotFound();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var band = await _context.Bands.FindAsync(BandId);
            if (band == null)
                return NotFound();

            NewRepertoire.BandId = BandId;
            _context.Repertoires.Add(NewRepertoire);
            await _context.SaveChangesAsync();

            return RedirectToPage("/BandPages/Details", new { id = BandId });
        }
    }
}
