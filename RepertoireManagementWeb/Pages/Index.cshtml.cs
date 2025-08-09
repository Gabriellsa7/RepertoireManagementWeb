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
                var user = _context.Users.FirstOrDefault(u => u.Name == User.Identity.Name);

                if (user != null)
                {
                    AllBands = _context.Bands
                        .Where(b => b.LeaderId == user.Id)
                        .ToList();

                    AllRepertoires = _context.Repertoires
                        .Include(r => r.Band)
                        .Where(r => r.Band != null && r.Band.LeaderId == user.Id)
                        .ToList();

                    AllMusic = _context.Musics
                        .Include(m => m.RepertoireLinks)
                        .ThenInclude(rm => rm.Repertoire)
                        .ThenInclude(r => r.Band)
                        .Where(m => m.RepertoireLinks.Any(rm => rm.Repertoire.Band != null && rm.Repertoire.Band.LeaderId == user.Id))
                        .ToList();
                }
            }
        }
    }
}
