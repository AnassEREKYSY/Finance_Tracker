using System;
using Core.Entities;
using Core.IServices;
using Infrastructure.Data;
using Core.Dtos;
using Microsoft.EntityFrameworkCore;
using Core.Response;

namespace Infrastructure.Services;

public class NotificationService(StoreContext _context, IUserService userService) : INotificationService
{
    public async Task<ServiceResponse<CreateUpdateNotifications>> CreateNotificationAsync(CreateUpdateNotifications notification,string userId)
    {
        var user = await userService.GetUserByIdAsync(userId);
        if (user == null)
        {
            return GetReponse(false,"User not Found.");
        }
        if (notification == null)
        {
            return GetReponse(false,"Invalid notification data.");
        }
        var createdNotification = new Notification{
            Message=notification.Message,
            IsRead=false,
            CreatedAt= DateTime.Now,
            UserId=userId,
            User=user
        };
        _context.Notifications.Add(createdNotification);
        await _context.SaveChangesAsync();
        notification.UserId= createdNotification.UserId;

        return GetReponse(true,"Notification Created Successfully",notification);
    }

    public async Task<ServiceResponse<IEnumerable<CreateUpdateNotifications>>> GetNotificationsByUserIdAsync(string userId)
    {
        var response = new ServiceResponse<IEnumerable<CreateUpdateNotifications>>();
        if (string.IsNullOrEmpty(userId))
        {
            response.Success = false;
            response.Message = "User ID cannot be null or empty.";
            return response;
        }
        var searchedNotifications = await _context.Notifications
            .AsNoTracking()
            .Include(n => n.User)
            .Where(n => n.UserId == userId)
            .ToListAsync();

        if (searchedNotifications == null || searchedNotifications.Count == 0)
        {
            response.Success = false;
            response.Message = $"No Notification found for the user with ID {userId}.";
            return response;
        }

        response.Data = searchedNotifications.Select(n => new CreateUpdateNotifications
        {
            NotificationId = n.NotificationId,
            UserId = n.UserId,
            Message = n.Message,
            IsRead = n.IsRead,
            CreatedAt = n.CreatedAt
        }).ToList();
        response.Success=true;
        return response;
    }

    public async Task<bool> MarkAsReadAsync(int id)
    {
        var response = new ServiceResponse<bool>();

        var notification = await _context.Notifications.FindAsync(id);
        if (notification != null)
        {
            notification.IsRead = true;
            await _context.SaveChangesAsync();

            return true;
        }

        throw new KeyNotFoundException($"Notification with id {id} not found.");
    }

    private ServiceResponse<CreateUpdateNotifications> GetReponse(bool Success, string? message=null,CreateUpdateNotifications? Data=null)
    {
        var response = new ServiceResponse<CreateUpdateNotifications>
        {
            Data= Data,
            Success = Success,
            Message = message!
        };
        return response;
    }
}
