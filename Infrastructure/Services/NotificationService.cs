using System;
using Core.Entities;
using Core.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class NotificationService(StoreContext _context) : INotificationService
{
    public async Task<Notification> CreateNotificationAsync(Notification notification)
    {
        if (notification != null)
        {
            notification.IsRead=false;
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        throw new ArgumentNullException(nameof(notification));
    }

    public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId)
    {
        return await _context.Notifications.Where(n => n.UserId == userId).ToListAsync();
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();
            return true;
        }

        throw new KeyNotFoundException($"Notification with id {id} not found.");
    }
}
