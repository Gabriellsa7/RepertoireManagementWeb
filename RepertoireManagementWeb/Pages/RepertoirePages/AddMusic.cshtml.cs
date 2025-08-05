using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

namespace RepertoireManagementWeb.Pages.RepertoirePages
{
    public class AddMusicModel : PageModel
    {
        private readonly AppDbContext _context;

        public AddMusicModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Guid RepertoireId { get; set; }

        [BindProperty]
        public Guid MusicId { get; set; }

        public string RepertoireName { get; set; } = "";
        public List<SelectListItem> MusicOptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid repertoireId)
        {
            var userIdStr = HttpContext.Session.GetString("UserId");
            if (!Guid.TryParse(userIdStr, out Guid userId))
                return Unauthorized();

            var repertoire = await _context.Repertoires
                .Include(r => r.Band)
                    .ThenInclude(b => b.Members)
                .FirstOrDefaultAsync(r =>
                    r.Id == repertoireId &&
                    (
                        r.Band.LeaderId == userId ||
                        r.Band.Members.Any(m => m.Id == userId)
                    ));

            if (repertoire == null)
                return NotFound();

            RepertoireId = repertoireId;
            RepertoireName = repertoire.Name;

            // Searching for musics that still don't have a repertoire
            var linkedMusicIds = await _context.RepertoireMusics
                .Where(rm => rm.RepertoireId == repertoireId)
                .Select(rm => rm.MusicId)
                .ToListAsync();

            var availableMusics = await _context.Musics
                .Where(m => !linkedMusicIds.Contains(m.Id))
                .ToListAsync();

            MusicOptions = availableMusics
                .Select(m => new SelectListItem { Value = m.Id.ToString(), Text = m.Title })
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            var alreadyExists = await _context.RepertoireMusics
                .AnyAsync(rm => rm.RepertoireId == RepertoireId && rm.MusicId == MusicId);

            if (alreadyExists)
                return RedirectToPage("Index");

            var newLink = new RepertoireMusic
            {
                Id = Guid.NewGuid(),
                RepertoireId = RepertoireId,
                MusicId = MusicId
            };

            _context.RepertoireMusics.Add(newLink);
            await _context.SaveChangesAsync();

            return RedirectToPage("Index");
        }
    }
}
