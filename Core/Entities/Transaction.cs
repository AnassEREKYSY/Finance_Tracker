using System;
using Core.Enums;

namespace Core.Entities;

public class Transaction
{
    public int TransactionId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public Category? Category { get; set; } = null!;
    public AppUser User { get; set; } = null!;
}

