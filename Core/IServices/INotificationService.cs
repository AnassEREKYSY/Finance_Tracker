using System;
using Core.Dtos;
using Core.Entities;
using Core.Response;

namespace Core.IServices;

public interface INotificationService
{
    Task<ServiceResponse<CreateUpdateNotifications>> CreateNotificationAsync(CreateUpdateNotifications notification,string userId);
    Task<ServiceResponse<IEnumerable<CreateUpdateNotifications>>> GetNotificationsByUserIdAsync(string userId);
    Task<bool> MarkAsReadAsync(int id);
}
