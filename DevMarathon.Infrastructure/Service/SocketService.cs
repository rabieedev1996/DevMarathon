using DevMarathon.Api.SignalR;
using DevMarathon.Application.Contract.Services;
using Microsoft.AspNetCore.SignalR;

namespace DevMarathon.Infrastructure.Service;
public class SocketService:ISocketService
{
    IHubContext<SignalRHub> _hubContext;

    public SocketService(IHubContext<SignalRHub> signalContext)
    {
        _hubContext = signalContext;
    }

    public async Task Broadcast<T>(string title,T data)
    {
        await _hubContext.Clients.All.SendAsync(title, data);
    }
    public async Task PushToClient<T>(string connectionId,string title,T data)
    {
        await _hubContext.Clients.Client(connectionId).SendAsync(title, data);
    }
    public async Task PushToClient<T>(List<string> connectionIds, string title, T data)
    {
        foreach (var connectionId in connectionIds)
        {
            await _hubContext.Clients.Client(connectionId).SendAsync(title, data);
        }
    }
}