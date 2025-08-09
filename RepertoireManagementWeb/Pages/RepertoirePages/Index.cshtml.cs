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
    public class IndexModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public IndexModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Repertoire> Repertoire { get;set; } = default!;

        public bool IsLeader { get; set; } = false;

        public async Task OnGetAsync()
        {
            var userIdStr = HttpContext.Session.GetString("UserId");

            if (!Guid.TryParse(userIdStr, out Guid userId))
            {
                Repertoire = new List<Repertoire>();
                IsLeader = false;
                return;
            }

            Repertoire = await _context.Repertoires
                .Include(r => r.Band)
                    .ThenInclude(b => b.Members)
                .Where(r =>
                    r.Band != null &&
                    (
                        r.Band.LeaderId == userId ||              
                        r.Band.Members.Any(m => m.Id == userId)    
                    )
                )
                .ToListAsync();

            IsLeader = await _context.Bands.AnyAsync(b => b.LeaderId == userId);
        }
    }
}
