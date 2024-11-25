using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
[Route("api/[controller]")]
[ApiController]
public class NotificationsController : ControllerBase
{
    private readonly IHubContext<NotificationHub> _hubContext;
    private static List<Notification> notifications = new List<Notification>();

    public NotificationsController(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Notification>> GetNotifications()
    {
        return Ok(notifications);
    }

    [HttpPost]
    public async Task<ActionResult> CreateNotification([FromBody] Notification notification)
    {
        notification.Id = notifications.Count + 1;
        notification.CreatedAt = DateTime.UtcNow;
        notifications.Add(notification);

        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);

        return Ok(notification);
    }
}

public class NotificationHub : Hub
{
}

