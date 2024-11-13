using System;
using Core.Dtos;
using Core.Entities;

namespace Core.IServices;

public interface IBudgetService
{
    Task<Budget> CreateBudgetAsync(CreateUpdateBudget budget, string userId);
    Task<CreateUpdateBudget> GetBudgetByIdAsync(int id,string userId);
    Task<IEnumerable<CreateUpdateBudget>> GetBudgetsAsync(string userId);
    Task<CreateUpdateBudget> UpdateBudgetAsync(int id, CreateUpdateBudget budget, string userId);
    Task<bool> DeleteBudgetAsync(int id, string userId);
}
