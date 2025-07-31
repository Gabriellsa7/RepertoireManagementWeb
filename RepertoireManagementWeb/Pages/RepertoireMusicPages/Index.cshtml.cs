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
            RepertoireMusic = await _context.RepertoireMusics
                .Include(r => r.Music)
                .Include(r => r.Repertoire).ToListAsync();
        }
    }
}
