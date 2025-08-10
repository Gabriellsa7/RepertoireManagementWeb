using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public List<Band> AllBands { get; set; } = new();
        public List<Repertoire> AllRepertoires { get; set; } = new();
        public List<Music> AllMusic { get; set; } = new();

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var user = _context.Users
                    .Include(u => u.Bands) // Ensure EF loads Bands navigation
                    .FirstOrDefault(u => u.Name == User.Identity.Name);

                if (user != null)
                {
                    // Bands where the user is Leader OR a Member
                    AllBands = _context.Bands
                        .Include(b => b.Members)
                        .Where(b => b.LeaderId == user.Id || b.Members.Any(m => m.Id == user.Id))
                        .ToList();

                    // Repertoires from those bands
                    AllRepertoires = _context.Repertoires
                        .Include(r => r.Band)
                        .Where(r => r.Band != null &&
                                    (r.Band.LeaderId == user.Id || r.Band.Members.Any(m => m.Id == user.Id)))
                        .ToList();

                    // Music from repertoires in those bands
                    AllMusic = _context.Musics
                        .Include(m => m.RepertoireLinks)
                            .ThenInclude(rm => rm.Repertoire)
                                .ThenInclude(r => r.Band)
                        .Where(m => m.RepertoireLinks.Any(rm => rm.Repertoire.Band != null &&
                                    (rm.Repertoire.Band.LeaderId == user.Id || rm.Repertoire.Band.Members.Any(mb => mb.Id == user.Id))))
                        .ToList();
                }
            }
        }

    }
}
