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
    public class IndexModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public IndexModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Music> Music { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (!Guid.TryParse(userIdStr, out Guid userId))
            {
                Music = new List<Music>();
                return;
            }

            Music = await _context.Musics
                .Include(m => m.RepertoireLinks)
                .ThenInclude(rl => rl.Repertoire)
                .ThenInclude(r => r.Band)
                .Where(m =>
                    m.RepertoireLinks.Any(rm =>
                        rm.Repertoire.Band != null &&
                        (
                            rm.Repertoire.Band.LeaderId == userId ||
                            rm.Repertoire.Band.Members.Any(mb => mb.Id == userId)
                        )
                    )
                )
                .Distinct()
                .ToListAsync();
        }

    }
}
