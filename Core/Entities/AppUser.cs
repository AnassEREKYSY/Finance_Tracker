using System;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities;

public class AppUser: IdentityUser
{
    public string FirstName { get; set; }  = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<Transaction> Transactions { get; set; }  = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
}
