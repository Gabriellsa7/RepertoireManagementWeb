using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace RepertoireManagementWeb.Hubs
{
    public class ShowHub : Hub
    {
        public async Task ChangeSong(Guid bandId, string title, string lyrics)
        {
            await Clients.Group(bandId.ToString())
                .SendAsync("ReceiveSong", title, lyrics);
        }

        public async Task JoinBand(Guid bandId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, bandId.ToString());
        }

    }
}
