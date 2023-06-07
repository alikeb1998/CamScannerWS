using Microsoft.AspNetCore.SignalR;

namespace CamScanner.Services;

public class MainHub:Hub
{
    public async Task AddToOnlineUserGroup(string id)
    {
        if (id is not null)
        {
           
            await Groups.AddToGroupAsync(Context.ConnectionId, id);
        }
    }

    public async Task RemoveFromOnlineUserGroup(string id)
    {
        if (id is not null)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, id);
        }
    }
    public override async Task OnConnectedAsync()
    {
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}