using System;
using Core.Entities;

namespace Core.Dtos;

public class BudgetUpdatedDto
{
    public int BudgetId { get; set; }
    public required string UserId { get; set; } 
    public required Category Category { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
