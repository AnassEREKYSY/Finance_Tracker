using System;
using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CategoryService(StoreContext _context) : ICategoryService
{

    public async Task<CreateUpdateCategory> CreateCategoryAsync(CreateUpdateCategory category)
    {
        if (category != null)
        {
            var createdCategory = new Category
            {
                Transactions= [],
                Budgets = [],
                Name=category.Name,
            };
            _context.Categories.Add(createdCategory);
            await _context.SaveChangesAsync();
            category.CategoryId=createdCategory.CategoryId;
            return category;
        }

        throw new ArgumentNullException(nameof(category));
    }

    public async Task<CreateUpdateCategory> GetCategoryByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id);
         if(category != null)
        {
            var searchedCategory= new CreateUpdateCategory
            {
                CategoryId=category.CategoryId,
                Name=category.Name
            };
            return searchedCategory;
        }
        throw new KeyNotFoundException($"Category with id {id} not found.");
    }

    public async Task<IEnumerable<CreateUpdateCategory>> GetCategoriesAsync()
    {
        var categories = await _context.Categories.ToListAsync();
        if(categories == null || categories.Count ==0)
        {
            throw new KeyNotFoundException($"No CAtegories found.");
        }
        var CatDtos = categories.Select(
            c => new CreateUpdateCategory
                {
                    Name=c.Name,
                    CategoryId=c.CategoryId
                }
            ).ToList();
        return CatDtos;
    }

    public async Task<CreateUpdateCategory> UpdateCategoryAsync(int id, CreateUpdateCategory category)
    {
        if (category != null)
        {
            var existingCategory = await _context.Categories.FindAsync(id) ?? throw new KeyNotFoundException($"Category with id {id} not found.");
            existingCategory.Name = category.Name;
            await _context.SaveChangesAsync();
            category.CategoryId=existingCategory.CategoryId;
            return category;
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

    public async Task<Category> GetCategoryByNameAsync(string Name)
    {
        var category = await _context.Categories.Where(c => c.Name == Name).FirstOrDefaultAsync();
        return category ?? throw new KeyNotFoundException($"Category with name {Name} not found.");
    }
}
