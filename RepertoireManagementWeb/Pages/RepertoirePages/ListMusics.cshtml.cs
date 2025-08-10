using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.RepertoirePages.MusicPage
{
    public class ListMusicsModel : PageModel
    {
        private readonly AppDbContext _context;

        public ListMusicsModel(AppDbContext context)
        {
            _context = context;
        }

        public Repertoire Repertoire { get; set; } = null!;
        public List<RepertoireMusic> MusicLinks { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid repertoireId)
        {
            Repertoire = await _context.Repertoires
                .Include(r => r.MusicLinks)
                    .ThenInclude(rm => rm.Music)
                .FirstOrDefaultAsync(r => r.Id == repertoireId);

            if (Repertoire == null)
                return NotFound();

            MusicLinks = Repertoire.MusicLinks
                .Where(rm => rm.IsActive)
                .OrderBy(rm => rm.OrderInRepertoire)
                .ToList();


            return Page();
        }
    }
}
