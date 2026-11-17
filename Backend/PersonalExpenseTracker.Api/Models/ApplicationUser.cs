
using Microsoft.AspNetCore.Identity;

namespace PersonalExpenseTracker.Api.Models;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public ICollection<Category> Categories { get; set; }
        = new List<Category>();
}
