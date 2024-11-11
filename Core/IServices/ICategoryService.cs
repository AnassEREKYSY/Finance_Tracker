using System;
using Core.Entities;

namespace Core.IServices;

public interface ICategoryService
{
    Task<Category> CreateCategoryAsync(Category category);
    Task<Category> GetCategoryByIdAsync(int id);
    Task<IEnumerable<Category>> GetCategoriesAsync();
    Task<Category> UpdateCategoryAsync(int id, Category category);
    Task<bool> DeleteCategoryAsync(int id);
}
