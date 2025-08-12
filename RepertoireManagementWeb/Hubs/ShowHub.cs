using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using RepertoireManagementWeb.Data;
using System;
using System.Threading.Tasks;

namespace RepertoireManagementWeb.Hubs
{
    public class ShowHub : Hub
    {
        private readonly AppDbContext _context;

        public ShowHub(AppDbContext context)
        {
            _context = context;
        }

        public async Task ChangeSong(Guid bandId, Guid musicId)
        {
            var music = await _context.Musics.FirstOrDefaultAsync(m => m.Id == musicId);
            if (music == null)
                return;

            string pdfUrl = $"/MusicPages/PdfDownload?id={music.Id}";

            await Clients.Group(bandId.ToString())
                .SendAsync("ReceiveSong", music.Title, pdfUrl);
        }

        public async Task JoinBand(Guid bandId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, bandId.ToString());
        }
    }
}
