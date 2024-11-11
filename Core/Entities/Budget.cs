using System;

namespace Core.Entities;

public class Budget
{
    public int BudgetId { get; set; }
    public string UserId { get; set; }  = string.Empty;
    public int CategoryId { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; } = DateTime.UtcNow;
    public DateTime EndDate { get; set; } = DateTime.UtcNow;
    public AppUser User { get; set; } = null!;
    public Category Category { get; set; } = null!;
}

