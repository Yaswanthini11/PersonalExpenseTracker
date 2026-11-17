using PersonalExpenseTracker.Api.DTOs.Categories;
using PersonalExpenseTracker.Api.Models;
using PersonalExpenseTracker.Api.Repositories.Interfaces;
using PersonalExpenseTracker.Api.Services.Interfaces;

namespace PersonalExpenseTracker.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryResponse>> GetAllAsync(string userId)
    {
        var categories =
            await _categoryRepository.GetAllForUserAsync(userId);

        return categories.Select(MapToResponse).ToList();
    }

    public async Task<CategoryResponse> CreateAsync(
        CreateCategoryRequest request,
        string userId)
    {
        string name = request.Name.Trim();

        bool nameExists =
            await _categoryRepository.NameExistsAsync(name, userId);

        if (nameExists)
        {
            throw new InvalidOperationException(
                "A category with this name already exists.");
        }

        var category = new Category
        {
            Name = name,
            Description = request.Description?.Trim(),
            UserId = userId
        };

        var createdCategory =
            await _categoryRepository.CreateAsync(category);

        return MapToResponse(createdCategory);
    }

    public async Task<CategoryResponse> UpdateAsync(
        int categoryId,
        UpdateCategoryRequest request,
        string userId)
    {
        var category =
            await _categoryRepository.GetCustomByIdAsync(
                categoryId,
                userId);

        if (category == null)
        {
            throw new KeyNotFoundException(
                "Custom category not found.");
        }

        string name = request.Name.Trim();

        bool nameExists =
            await _categoryRepository.NameExistsAsync(
                name,
                userId,
                categoryId);

        if (nameExists)
        {
            throw new InvalidOperationException(
                "A category with this name already exists.");
        }

        category.Name = name;
        category.Description = request.Description?.Trim();

        await _categoryRepository.UpdateAsync(category);

        return MapToResponse(category);
    }

    public async Task<bool> DeleteAsync(
        int categoryId,
        string userId)
    {
        var category =
            await _categoryRepository.GetCustomByIdAsync(
                categoryId,
                userId);

        if (category == null)
        {
            return false;
        }

        await _categoryRepository.DeleteAsync(category);

        return true;
    }

    private static CategoryResponse MapToResponse(Category category)
    {
        return new CategoryResponse
        {
            CategoryId = category.CategoryId,
            Name = category.Name,
            Description = category.Description,
            IsDefault = category.UserId == null
        };
    }
}