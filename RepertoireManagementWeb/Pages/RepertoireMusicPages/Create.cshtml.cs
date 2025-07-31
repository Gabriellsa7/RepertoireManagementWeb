using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.RepertoireMusicPages
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
        ViewData["MusicId"] = new SelectList(_context.Musics, "Id", "Title");
        ViewData["RepertoireId"] = new SelectList(_context.Repertoires, "Id", "Description");
            return Page();
        }

        [BindProperty]
        public RepertoireMusic RepertoireMusic { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.RepertoireMusics.Add(RepertoireMusic);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
