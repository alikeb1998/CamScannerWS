
using CamScanner.Models;
using MassTransit;
using Microsoft.AspNetCore.SignalR;

namespace CamScanner.Services;

public class MessageConsumer : IConsumer<Image>
{
    private readonly IHubContext<MainHub> _hub;
    private readonly CancellationTokenSource _cancellation = new();
    public MessageConsumer(IHubContext<MainHub> hub)
    {
        _hub = hub;
    }

    public  Task Consume(ConsumeContext<Image> context)
    {
        Console.WriteLine($"Received message time {DateTime.UtcNow}: {context.Message.Data}");
        
         _hub.Clients.Group(context.Message.ConnectionId)
            .SendAsync("OnRefreshPrice", context.Message.Data, _cancellation.Token);
        return Task.CompletedTask;
    }
}
