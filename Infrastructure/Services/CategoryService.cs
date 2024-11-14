using System;
using Core.Dtos;
using Core.Entities;
using Core.IServices;
using Core.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CategoryService(StoreContext _context) : ICategoryService
{

        public async Task<ServiceResponse<CreateUpdateCategory>> CreateCategoryAsync(CreateUpdateCategory category)
        {
            var response = new ServiceResponse<CreateUpdateCategory>();

            if (category != null)
            {
                var createdCategory = new Category
                {
                    Transactions = [], 
                    Budgets = [],           
                    Name = category.Name
                };

                _context.Categories.Add(createdCategory);
                await _context.SaveChangesAsync();

                category.CategoryId = createdCategory.CategoryId;
                response.Data = category;
                response.Success=true;
                response.Message="Category created Successfully";
                return response;
            }

            response.Success = false;
            response.Message = "Category data is invalid.";
            return response;
        }

        public async Task<ServiceResponse<CreateUpdateCategory>> GetCategoryByIdAsync(int id)
        {
            var response = new ServiceResponse<CreateUpdateCategory>();

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                response.Data = new CreateUpdateCategory
                {
                    CategoryId = category.CategoryId,
                    Name = category.Name
                };
                response.Success=true;
                return response;
            }

            response.Success = false;
            response.Message = $"Category with id {id} not found.";
            return response;
        }


        public async Task<ServiceResponse<IEnumerable<CreateUpdateCategory>>> GetCategoriesAsync()
        {
            var response = new ServiceResponse<IEnumerable<CreateUpdateCategory>>();

            var categories = await _context.Categories.ToListAsync();
            if (categories == null || categories.Count == 0)
            {
                response.Success = false;
                response.Message = "No categories found.";
                return response;
            }

            response.Data = categories.Select(c => new CreateUpdateCategory
            {
                CategoryId = c.CategoryId,
                Name = c.Name
            }).ToList();
            response.Success=true;

            return response;
        }

        public async Task<ServiceResponse<CreateUpdateCategory>> UpdateCategoryAsync(int id, CreateUpdateCategory category)
        {
            var response = new ServiceResponse<CreateUpdateCategory>();

            if (category != null)
            {
                var existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null)
                {
                    response.Success = false;
                    response.Message = $"Category with id {id} not found.";
                    return response;
                }

                existingCategory.Name = category.Name;
                await _context.SaveChangesAsync();

                category.CategoryId = existingCategory.CategoryId;
                response.Success = true;
                response.Data = category;
                return response;
            }

            response.Success = false;
            response.Message = "Category data is invalid.";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteCategoryAsync(int id)
        {
            var response = new ServiceResponse<bool>();

            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                response.Data = true;
                return response;
            }

            response.Success = false;
            response.Message = $"Category with id {id} not found.";
            return response;
        }

    public async Task<Category> GetCategoryByNameAsync(string Name)
    {
        var category = await _context.Categories.Where(c => c.Name == Name).FirstOrDefaultAsync();
        return category ?? throw new KeyNotFoundException($"Category with name {Name} not found.");
    }
}
