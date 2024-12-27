using System;
using Core.Dtos;
using Core.Entities;
using Core.Response;

namespace Core.IServices;

public interface ICategoryService
{
    Task<ServiceResponse<CreateUpdateCategory>> CreateCategoryAsync(CreateUpdateCategory category);
    Task<ServiceResponse<CreateUpdateCategory>> GetCategoryByIdAsync(int id);
    Task<Category> GetCategoryByNameAsync(string Name);
    Task<ServiceResponse<IEnumerable<CreateUpdateCategory>>> GetCategoriesAsync();
    Task<ServiceResponse<CreateUpdateCategory>>  UpdateCategoryAsync(int id, CreateUpdateCategory category);
    Task<ServiceResponse<bool>> DeleteCategoryAsync(int id);
}
