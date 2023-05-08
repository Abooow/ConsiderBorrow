using Microsoft.EntityFrameworkCore;

namespace ConsiderBorrow.Server.DataAccess;

internal sealed class ApplicationDbContext : DbContext
{
    public DbSet<EmployeeRecord> Employees => Set<EmployeeRecord>();
    public DbSet<LibraryItemRecord> LibraryItems => Set<LibraryItemRecord>();
    public DbSet<CategoryRecord> Categories => Set<CategoryRecord>();

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : base(options)
    {
    }
}
