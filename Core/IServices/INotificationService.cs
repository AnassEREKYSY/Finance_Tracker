using System;
using Core.Entities;

namespace Core.IServices;

public interface INotificationService
{
    Task<Notification> CreateNotificationAsync(Notification notification);
    Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(string userId);
    Task<bool> MarkAsReadAsync(int id);
}
