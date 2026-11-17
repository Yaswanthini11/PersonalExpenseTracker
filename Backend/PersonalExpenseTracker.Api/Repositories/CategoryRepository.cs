using Microsoft.EntityFrameworkCore;
using PersonalExpenseTracker.Api.Data;
using PersonalExpenseTracker.Api.Models;
using PersonalExpenseTracker.Api.Repositories.Interfaces;

namespace PersonalExpenseTracker.Api.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;

    public CategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllForUserAsync(string userId)
    {
        return await _context.Categories
            .Where(c => c.UserId == null || c.UserId == userId)
            .OrderBy(c => c.Name)
            .ToListAsync();
    }

    public async Task<Category?> GetCustomByIdAsync(
        int categoryId,
        string userId)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c =>
                c.CategoryId == categoryId &&
                c.UserId == userId);
    }

    public async Task<bool> NameExistsAsync(
        string name,
        string userId,
        int? excludeCategoryId = null)
    {
        return await _context.Categories.AnyAsync(c =>
            c.Name == name &&
            (c.UserId == null || c.UserId == userId) &&
            (excludeCategoryId == null ||
             c.CategoryId != excludeCategoryId.Value));
    }

    public async Task<Category> CreateAsync(Category category)
    {
        await _context.Categories.AddAsync(category);
        await _context.SaveChangesAsync();

        return category;
    }

    public async Task UpdateAsync(Category category)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Category category)
    {
        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}