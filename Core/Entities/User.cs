using System;
using Microsoft.AspNetCore.Identity;
namespace Core.Entities;

public class User: IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Transaction> Transactions { get; set; } = null!;
    public ICollection<Budget> Budgets { get; set; } = null!;
}
