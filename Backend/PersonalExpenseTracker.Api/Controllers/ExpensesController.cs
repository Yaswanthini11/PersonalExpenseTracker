using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenseTracker.Api.DTOs.Expenses;
using PersonalExpenseTracker.Api.Services;
using PersonalExpenseTracker.Api.Services.Interfaces;

namespace PersonalExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/expenses")]
[Authorize]
public class ExpensesController : ControllerBase
{
    private readonly IExpenseService _expenseService;

    public ExpensesController(IExpenseService expenseService)
    {
        _expenseService = expenseService;
    }

    private string? GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    // GET: api/expenses
    // GET: api/expenses?month=11&year=2026
    // GET: api/expenses?categoryId=1
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] int? month,
        [FromQuery] int? year,
        [FromQuery] int? categoryId)
    {
        if (month.HasValue && (month < 1 || month > 12))
        {
            return BadRequest("Month must be between 1 and 12.");
        }

        if (year.HasValue && (year < 1 || year > 9999))
        {
            return BadRequest("Invalid year.");
        }

        if (categoryId.HasValue && categoryId < 1)
        {
            return BadRequest("Invalid category ID.");
        }

        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var expenses = await _expenseService.GetAllAsync(
            userId,
            month,
            year,
            categoryId);

        return Ok(expenses);
    }

    // GET: api/expenses/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var expense = await _expenseService.GetByIdAsync(id, userId);

        if (expense == null)
        {
            return NotFound("Expense not found.");
        }

        return Ok(expense);
    }

    // POST: api/expenses
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateExpenseRequest request)
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var expense = await _expenseService.CreateAsync(
            request,
            userId);

        if (expense == null)
        {
            return BadRequest(
                "The selected category does not exist or is not available to you.");
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = expense.ExpenseId },
            expense);
    }

    // PUT: api/expenses/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateExpenseRequest request)
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var result = await _expenseService.UpdateAsync(
            id,
            request,
            userId);

        if (result == ExpenseUpdateResult.NotFound)
        {
            return NotFound("Expense not found.");
        }

        if (result == ExpenseUpdateResult.InvalidCategory)
        {
            return BadRequest(
                "The selected category does not exist or is not available to you.");
        }

        var updatedExpense = await _expenseService.GetByIdAsync(
            id,
            userId);

        return Ok(updatedExpense);
    }

    // DELETE: api/expenses/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();

        if (string.IsNullOrEmpty(userId))
        {
            return Unauthorized();
        }

        var deleted = await _expenseService.DeleteAsync(id, userId);

        if (!deleted)
        {
            return NotFound("Expense not found.");
        }

        return NoContent();
    }
}