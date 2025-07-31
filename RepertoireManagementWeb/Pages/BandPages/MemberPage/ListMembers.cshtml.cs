using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.BandPages.MemberPage
{
    public class ListMembersModel : PageModel
    {
        private readonly AppDbContext _context;

        public ListMembersModel(AppDbContext context)
        {
            _context = context;
        }

        public Band Band { get; set; } = null!;
        public List<User> Members { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid bandId)
        {
            Band = await _context.Bands
                .Include(b => b.Members)
                .FirstOrDefaultAsync(b => b.Id == bandId);

            if (Band == null)
                return NotFound();

            Members = Band.Members.ToList();
            return Page();
        }
    }
}
