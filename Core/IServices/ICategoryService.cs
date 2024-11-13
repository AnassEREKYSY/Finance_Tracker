using System;
using Core.Dtos;
using Core.Entities;

namespace Core.IServices;

public interface ICategoryService
{
    Task<CreateUpdateCategory> CreateCategoryAsync(CreateUpdateCategory category);
    Task<CreateUpdateCategory> GetCategoryByIdAsync(int id);
    Task<Category> GetCategoryByNameAsync(string Name);
    Task<IEnumerable<CreateUpdateCategory>> GetCategoriesAsync();
    Task<CreateUpdateCategory> UpdateCategoryAsync(int id, CreateUpdateCategory category);
    Task<bool> DeleteCategoryAsync(int id);
}
