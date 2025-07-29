using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.RepertoirePages
{
    public class CreateModel : PageModel
    {
        private readonly RepertoireManagementWeb.Data.AppDbContext _context;

        public CreateModel(RepertoireManagementWeb.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["BandId"] = new SelectList(_context.Bands, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Repertoire Repertoire { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Repertoires.Add(Repertoire);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
