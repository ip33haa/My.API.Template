using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

/// <summary>
/// SignalR hub for handling real-time notifications.
/// </summary>
[Authorize]
public class NotificationsHub : Hub<INotificationClient>
{
    /// <summary>
    /// Invoked when a client connects to the hub.
    /// Sends a welcome notification to the connected user.
    /// </summary>
    public override async Task OnConnectedAsync()
    {
        var userName = Context.User?.Identity?.Name ?? "User";
        var message = $"Welcome, {userName}! You are now connected.";

        await Clients.Client(Context.ConnectionId).ReceiveNotification(message);
        await base.OnConnectedAsync();
    }
}

/// <summary>
/// Interface defining client-side methods that the hub can call.
/// </summary>
public interface INotificationClient
{
    /// <summary>
    /// Sends a notification message to the client.
    /// </summary>
    /// <param name="message">The notification message.</param>
    Task ReceiveNotification(string message);
}