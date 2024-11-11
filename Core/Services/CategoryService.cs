using System;
using Core.Entities;
using Core.Exceptions;
using Core.IData;
using Core.IServices;
using Microsoft.EntityFrameworkCore;

namespace Core.Services;

public class CategoryService(IStoreContext _context) : ICategoryService
{

    public async Task<Category> CreateCategoryAsync(Category category)
    {
        if (category != null)
        {
            category.Transactions ??= [];
            category.Budgets ??= [];
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        throw new ArgumentNullException(nameof(category));
    }

    public async Task<Category> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        return category ?? throw new KeyNotFoundException($"Category with id {id} not found.");
    }

    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category> UpdateCategoryAsync(int id, Category category)
    {
        if (category != null)
        {
            var existingCategory = await _context.Categories.FindAsync(id) ?? throw new KeyNotFoundException($"Category with id {id} not found.");
            existingCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            return existingCategory;
        }

        throw new ArgumentNullException(nameof(category));
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
        if (category != null)
        {
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }

        throw new KeyNotFoundException($"Category with id {id} not found.");
    }
}
