using Microsoft.EntityFrameworkCore;
using PersonalExpenseTracker.Api.Data;
using PersonalExpenseTracker.Api.Models;
using PersonalExpenseTracker.Api.Repositories.Interfaces;

namespace PersonalExpenseTracker.Api.Repositories;

public class ExpenseRepository : IExpenseRepository
{
    private readonly ApplicationDbContext _context;

    public ExpenseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Expense>> GetAllForUserAsync(
        string userId,
        int? month,
        int? year,
        int? categoryId)
    {
        var query = _context.Expenses
            .Include(e => e.Category)
            .Where(e => e.UserId == userId);

        if (month.HasValue)
        {
            query = query.Where(
                e => e.ExpenseDate.Month == month.Value);
        }

        if (year.HasValue)
        {
            query = query.Where(
                e => e.ExpenseDate.Year == year.Value);
        }

        if (categoryId.HasValue)
        {
            query = query.Where(
                e => e.CategoryId == categoryId.Value);
        }

        return await query
            .OrderByDescending(e => e.ExpenseDate)
            .ToListAsync();
    }

    public async Task<Expense?> GetByIdForUserAsync(
        int expenseId,
        string userId)
    {
        return await _context.Expenses
            .Include(e => e.Category)
            .FirstOrDefaultAsync(
                e => e.ExpenseId == expenseId &&
                     e.UserId == userId);
    }

    public async Task<bool> IsCategoryAvailableForUserAsync(
        int categoryId,
        string userId)
    {
        return await _context.Categories
            .AnyAsync(c =>
                c.CategoryId == categoryId &&
                (c.UserId == null || c.UserId == userId));
    }

    public async Task AddAsync(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
        await _context.SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expense expense)
    {
        _context.Expenses.Remove(expense);
        await _context.SaveChangesAsync();
    }
}