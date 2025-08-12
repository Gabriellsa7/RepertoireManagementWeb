using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using RepertoireManagementWeb.Models;

public class LeaderShowModel : PageModel
{
    private readonly AppDbContext _context;

    public LeaderShowModel(AppDbContext context)
    {
        _context = context;
    }

    [BindProperty(SupportsGet = true)]
    public Guid BandId { get; set; }

    [BindProperty(SupportsGet = true)]
    public Guid RepertoireId { get; set; }

    public string BandName { get; set; } = string.Empty;
    public string RepertoireName { get; set; } = string.Empty;
    public List<MusicViewModel> Musics { get; set; } = new();

    public async Task<IActionResult> OnGetAsync()
    {
        var band = await _context.Bands
            .Include(b => b.Repertoires)
                .ThenInclude(r => r.MusicLinks)
                    .ThenInclude(rm => rm.Music)
            .FirstOrDefaultAsync(b => b.Id == BandId);

        if (band == null)
            return NotFound("Banda não encontrada.");

        BandName = band.Name;

        var repertoire = band.Repertoires
            .FirstOrDefault(r => r.Id == RepertoireId);

        if (repertoire == null)
            return NotFound("Repertório não encontrado.");

        RepertoireName = repertoire.Name;

        Musics = repertoire.MusicLinks
            .Select(rm => new MusicViewModel
            {
                Title = rm.Music.Title,
                Lyrics = "Letra da música aqui..."
            })
            .ToList();

        return Page();
    }

    public class MusicViewModel
    {
        public string Title { get; set; }
        public string? Lyrics { get; set; }
    }
}
