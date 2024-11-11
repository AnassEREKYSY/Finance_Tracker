using System;
using Core.Dtos;
using Core.Entities;
using Core.Exceptions;
using Core.IData;
using Core.IServices;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class BudgetService(IStoreContext _context) : IBudgetService
{
    public async Task<Budget> CreateBudgetAsync(Budget budget, string userId)
    {
        if (budget != null)
        {
            budget.UserId=userId;
            _context.Budgets.Add(budget);
            await _context.SaveChangesAsync();
            return budget;
        }

        throw new ArgumentNullException(nameof(budget));
    }

    public async Task<Budget> GetBudgetByIdAsync(int id,string userId)
    {
        var budget = await _context.Budgets.Where(b => b.BudgetId == id && b.UserId == userId).FirstOrDefaultAsync();
        return budget ?? throw new KeyNotFoundException($"Budget with id {id} not found or does not belong to the user.");
    }

    public async Task<IEnumerable<Budget>> GetBudgetsAsync(string userId)
    {
        return await _context.Budgets.Where(b => b.UserId == userId).ToListAsync();
    }

    public async Task<Budget> UpdateBudgetAsync(int id, BudgetUpdatedDto budgetDto, string userId)
    {
        if (budgetDto != null)
        {
            var existingBudget = await _context.Budgets.FindAsync(id) ?? throw new KeyNotFoundException($"Budget with id {id} not found.");
            if (existingBudget.UserId == userId)
            {
                existingBudget.Amount = budgetDto.Amount;
                existingBudget.Category = budgetDto.Category;
                existingBudget.StartDate = budgetDto.StartDate;
                existingBudget.EndDate = budgetDto.EndDate;

                await _context.SaveChangesAsync();

                return existingBudget;
            }
            throw new UnauthorizedAccessException("You are not authorized to update this budget.");
        }

        throw new ArgumentNullException(nameof(budgetDto));
    }

    public async Task<bool> DeleteBudgetAsync(int id, string userId)
    {
        var budget = await _context.Budgets.FindAsync(id);
        if (budget != null)
        {
            if (budget.UserId != userId)
            {
                _context.Budgets.Remove(budget);
                await _context.SaveChangesAsync();
                return true;
            }
            throw new UnauthorizedAccessException("You are not authorized to delete this budget.");

        }
        throw new KeyNotFoundException($"Budget with id {id} not found.");
    }
}
