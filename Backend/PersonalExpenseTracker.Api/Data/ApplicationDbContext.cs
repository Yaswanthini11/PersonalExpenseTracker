
using Microsoft.EntityFrameworkCore;

namespace PersonalExpenseTracker.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}
