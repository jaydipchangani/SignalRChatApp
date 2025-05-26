using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    // Store user connections: username -> connectionId
    private static ConcurrentDictionary<string, string> userConnections = new();

    public override Task OnConnectedAsync()
    {
        // For demo, get username from query string
        var userName = Context.GetHttpContext().Request.Query["user"];

        if (!string.IsNullOrEmpty(userName))
        {
            userConnections[userName] = Context.ConnectionId;
        }

        return base.OnConnectedAsync();
    }

    public override Task OnDisconnectedAsync(System.Exception exception)
    {
        var userName = Context.GetHttpContext().Request.Query["user"];

        if (!string.IsNullOrEmpty(userName))
        {
            userConnections.TryRemove(userName, out _);
        }

        return base.OnDisconnectedAsync(exception);
    }

    // One-to-one message send
    public async Task SendPrivateMessage(string fromUser, string toUser, string message)
    {
        if (userConnections.TryGetValue(toUser, out var connectionId))
        {
            await Clients.Client(connectionId).SendAsync("ReceivePrivateMessage", fromUser, message);
        }
    }
}
