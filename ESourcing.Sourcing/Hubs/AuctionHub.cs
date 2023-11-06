using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ESourcing.Sourcing.Hubs
{
    public class AuctionHub :Hub
    {
        public async Task AddToGroup(string groupName)  //bidleri görecek grup
        {
            await Groups.AddToGroupAsync(Context.ConnectionId,groupName);
        }

        public async Task SendBidAsync(string groupName,string user,string bid)  //invoke methodu ile SendBidAsync, on ile Bids methodu cagırılıyor.
        {
            await Clients.Group(groupName).SendAsync("Bids", user, bid);
        }

    }
}
