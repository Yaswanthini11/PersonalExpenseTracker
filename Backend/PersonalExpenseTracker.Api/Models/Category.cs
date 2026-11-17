
namespace PersonalExpenseTracker.Api.Models;

public class Category
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }

    // Null means this is a shared default category.
    // A UserId means this is a category created by that user.
    public string? UserId { get; set; }

    public ApplicationUser? User { get; set; }
}
