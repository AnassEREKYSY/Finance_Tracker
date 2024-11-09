using System;

namespace Core.Entities;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public ICollection<Transaction> Transactions { get; set; } = null!;
    public ICollection<Budget> Budgets { get; set; } = null!;
}
