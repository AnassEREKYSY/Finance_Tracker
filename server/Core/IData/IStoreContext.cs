using System;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.IData;

    public interface IStoreContext
    {
        DbSet<Budget> Budgets { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<ExportRequest> ExportRequests { get; set; }
        DbSet<Notification> Notifications { get; set; }
        DbSet<Transaction> Transactions { get; set; }
        Task<int> SaveChangesAsync();
    }