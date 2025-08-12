using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RepertoireManagementWeb.Pages
{
    public class ShowModel : PageModel
    {
        private readonly AppDbContext _context;

        public ShowModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty(SupportsGet = true)]
        public Guid BandId { get; set; }

        [BindProperty(SupportsGet = true)]
        public Guid RepertoireId { get; set; }

        public string CurrentSongTitle { get; set; } = "Waiting...";
        public string CurrentSongPdfUrl { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            var bandExists = await _context.Bands
                .AnyAsync(b => b.Id == BandId);

            if (!bandExists)
                return NotFound("Band Not Found");

            var firstMusic = await _context.RepertoireMusics
                .Where(rm => rm.RepertoireId == RepertoireId)
                .Include(rm => rm.Music)
                .Select(rm => rm.Music)
                .FirstOrDefaultAsync();

            if (firstMusic != null)
            {
                CurrentSongTitle = firstMusic.Title;
                CurrentSongPdfUrl = $"/MusicPages/PdfDownload?id={firstMusic.Id}";
            }

            return Page();
        }
    }
}
