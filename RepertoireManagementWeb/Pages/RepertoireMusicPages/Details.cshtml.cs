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
    public class DetailsModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public DetailsModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        public RepertoireMusic RepertoireMusic { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repertoiremusic = await _context.RepertoireMusics.FirstOrDefaultAsync(m => m.Id == id);
            if (repertoiremusic == null)
            {
                return NotFound();
            }
            else
            {
                RepertoireMusic = repertoiremusic;
            }
            return Page();
        }
    }
}
