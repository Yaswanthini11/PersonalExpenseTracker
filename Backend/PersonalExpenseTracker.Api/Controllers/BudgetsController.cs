
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenseTracker.Api.DTOs.Budgets;
using PersonalExpenseTracker.Api.Services.Interfaces;

namespace PersonalExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/budgets")]
[Authorize]
public class BudgetsController : ControllerBase
{
    private readonly IBudgetService _budgetService;

    public BudgetsController(IBudgetService budgetService)
    {
        _budgetService = budgetService;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }

    // POST: api/budgets
    [HttpPost]
    public async Task<IActionResult> CreateBudget(
        CreateBudgetRequest request)
    {
        var userId = GetUserId();

        var result = await _budgetService.CreateOrUpdateAsync(
            request,
            userId);

        if (result.Created)
        {
            return CreatedAtAction(
                nameof(GetBudget),
                new
                {
                    year = result.Budget.Year,
                    month = result.Budget.Month
                },
                result.Budget);
        }

        return Ok(result.Budget);
    }

    // GET: api/budgets?year=2026&month=11
    [HttpGet]
    public async Task<IActionResult> GetBudget(
        [FromQuery] int year,
        [FromQuery] int month)
    {
        if (year < 1 || year > 9999)
        {
            return BadRequest("Invalid year.");
        }

        if (month < 1 || month > 12)
        {
            return BadRequest("Month must be between 1 and 12.");
        }

        var userId = GetUserId();

        var budget = await _budgetService.GetBudgetAsync(
            userId,
            year,
            month);

        if (budget == null)
        {
            return NotFound("Budget not found for the selected month.");
        }

        return Ok(budget);
    }

    // PUT: api/budgets
    [HttpPut]
    public async Task<IActionResult> UpdateBudget(
        UpdateBudgetRequest request)
    {
        var userId = GetUserId();

        var budget = await _budgetService.UpdateAsync(
            request,
            userId);

        if (budget == null)
        {
            return NotFound("Budget not found for the selected month.");
        }

        return Ok(budget);
    }
}
