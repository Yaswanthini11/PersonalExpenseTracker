using System.ComponentModel.DataAnnotations;

namespace PersonalExpenseTracker.Api.DTOs.Categories;

public class CreateCategoryRequest
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(200)]
    public string? Description { get; set; }
}