using System;
using Core.Dtos;
using Core.Entities;
using Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Core.IServices;

public interface IBudgetService
{
    Task<ServiceResponse<CreateUpdateBudget>> CreateBudgetAsync(CreateUpdateBudget budget, string userId);
    Task<ServiceResponse<CreateUpdateBudget>> GetBudgetByIdAsync(int id,string userId);
    Task<ServiceResponse<IEnumerable<CreateUpdateBudget>>> GetBudgetsAsync(string userId);
    Task<ServiceResponse<CreateUpdateBudget>> UpdateBudgetAsync(int id, CreateUpdateBudget budget, string userId);
    Task<ServiceResponse<bool>> DeleteBudgetAsync(int id, string userId);
    void CheckBudgetStatus(string userId);
}
