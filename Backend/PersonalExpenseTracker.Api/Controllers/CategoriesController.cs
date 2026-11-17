using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalExpenseTracker.Api.DTOs.Categories;
using PersonalExpenseTracker.Api.Services.Interfaces;

namespace PersonalExpenseTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    private string GetUserId()
    {
        return User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException(
                "User ID claim was not found.");
    }

    // GET: api/categories
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetUserId();

        var categories =
            await _categoryService.GetAllAsync(userId);

        return Ok(categories);
    }

    // POST: api/categories
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryRequest request)
    {
        try
        {
            var userId = GetUserId();

            var category =
                await _categoryService.CreateAsync(request, userId);

            return StatusCode(
                StatusCodes.Status201Created,
                category);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // PUT: api/categories/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryRequest request)
    {
        try
        {
            var userId = GetUserId();

            var category =
                await _categoryService.UpdateAsync(
                    id,
                    request,
                    userId);

            return Ok(category);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // DELETE: api/categories/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetUserId();

        bool deleted =
            await _categoryService.DeleteAsync(id, userId);

        if (!deleted)
        {
            return NotFound(new
            {
                message = "Custom category not found."
            });
        }

        return NoContent();
    }
}