using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.BandPages
{
    public class DetailsModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public DetailsModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        public Band Band { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var band = await _context.Bands.FirstOrDefaultAsync(m => m.Id == id);
            if (band == null)
            {
                return NotFound();
            }
            else
            {
                Band = band;
            }
            return Page();
        }
    }
}
