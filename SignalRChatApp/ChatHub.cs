using Microsoft.AspNetCore.SignalR;

public class ChatHub : Hub
{
    public async Task SendMessage(string user, string message)
    {
        Console.WriteLine($"Received: {user} - {message}");
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }

}
