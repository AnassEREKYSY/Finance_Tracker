using System;
using Core.Entities;

namespace Core.Dtos;

public class CreateUpdateNotifications
{
    public int NotificationId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public AppUser User { get; set; } = null!;
    public int? BudgetId { get; set; } 
    public Budget? Budget { get; set; }
    
}