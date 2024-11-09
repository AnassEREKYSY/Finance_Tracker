using System;

namespace Core.Entities;

public class Budget
{
    public int BudgetId { get; set; }
    public string UserId { get; set; }
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    public User User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}

