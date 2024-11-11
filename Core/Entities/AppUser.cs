using System;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities;

public class AppUser: IdentityUser
{
    public string FirstName { get; set; }  = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public ICollection<Transaction> Transactions { get; set; } = null!;
    public ICollection<Budget> Budgets { get; set; } = null!;
}
