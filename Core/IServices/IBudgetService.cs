using System;
using Core.Dtos;
using Core.Entities;

namespace Core.IServices;

public interface IBudgetService
{
    Task<Budget> CreateBudgetAsync(Budget budget, string userId);
    Task<Budget> GetBudgetByIdAsync(int id,string userId);
    Task<IEnumerable<Budget>> GetBudgetsAsync(string userId);
    Task<Budget> UpdateBudgetAsync(int id, BudgetUpdatedDto budget, string userId);
    Task<bool> DeleteBudgetAsync(int id, string userId);
}
