using PersonalExpenseTracker.Api.DTOs.Categories;

namespace PersonalExpenseTracker.Api.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync(string userId);

    Task<CategoryResponse> CreateAsync(
        CreateCategoryRequest request,
        string userId);

    Task<CategoryResponse> UpdateAsync(
        int categoryId,
        UpdateCategoryRequest request,
        string userId);

    Task<bool> DeleteAsync(
        int categoryId,
        string userId);
}