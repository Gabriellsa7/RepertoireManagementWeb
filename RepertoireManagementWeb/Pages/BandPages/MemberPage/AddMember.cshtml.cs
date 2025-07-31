using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.Bands.Members
{
    public class AddMemberModel : PageModel
    {
        private readonly AppDbContext _context;

        public AddMemberModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid BandId { get; set; }

        [BindProperty]
        public Guid SelectedUserId { get; set; }

        public Band Band { get; set; } = null!;
        public SelectList AvailableUsers { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync(Guid bandId)
        {
            Band = await _context.Bands
                .Include(b => b.Members)
                .FirstOrDefaultAsync(b => b.Id == bandId);

            if (Band == null)
                return NotFound();

            BandId = bandId;

            var membersIds = Band.Members.Select(m => m.Id).ToList();
            var users = await _context.Users
                .Where(u => !membersIds.Contains(u.Id))
                .ToListAsync();

            AvailableUsers = new SelectList(users, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var band = await _context.Bands
                .Include(b => b.Members)
                .FirstOrDefaultAsync(b => b.Id == BandId);

            var user = await _context.Users.FindAsync(SelectedUserId);

            if (band == null || user == null)
                return NotFound();

            if (!band.Members.Contains(user))
            {
                band.Members.Add(user);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/BandPages/Details", new { id = BandId });
        }
    }
}
