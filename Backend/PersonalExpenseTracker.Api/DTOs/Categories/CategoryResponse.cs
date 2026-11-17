namespace PersonalExpenseTracker.Api.DTOs.Categories;

public class CategoryResponse
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    public bool IsDefault { get; set; }
}