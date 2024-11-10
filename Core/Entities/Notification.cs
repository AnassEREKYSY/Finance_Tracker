using System;

namespace Core.Entities;

public class Notification
{
    public int NotificationId { get; set; }
    public string UserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public AppUser User { get; set; } = null!;
}

