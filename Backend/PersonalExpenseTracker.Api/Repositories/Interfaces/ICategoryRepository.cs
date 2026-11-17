using PersonalExpenseTracker.Api.Models;

namespace PersonalExpenseTracker.Api.Repositories.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllForUserAsync(string userId);

    Task<Category?> GetCustomByIdAsync(
        int categoryId,
        string userId);

    Task<bool> NameExistsAsync(
        string name,
        string userId,
        int? excludeCategoryId = null);

    Task<Category> CreateAsync(Category category);

    Task UpdateAsync(Category category);

    Task DeleteAsync(Category category);
}