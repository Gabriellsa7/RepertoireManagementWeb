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
    public class IndexModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public IndexModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<RepertoireMusic> RepertoireMusic { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (!Guid.TryParse(userIdStr, out Guid userId))
            {
                RepertoireMusic = new List<RepertoireMusic>();
                return;
            }

            RepertoireMusic = await _context.RepertoireMusics
                .Include(rm => rm.Music)
                .Include(rm => rm.Repertoire)
                    .ThenInclude(r => r.Band)
                        .ThenInclude(b => b.Members)
                .Where(rm =>
                    rm.Repertoire.Band != null &&
                    (
                        rm.Repertoire.Band.LeaderId == userId ||
                        rm.Repertoire.Band.Members.Any(m => m.Id == userId)
                    )
                )
                .ToListAsync();
        }

    }
}
