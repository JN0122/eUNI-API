using Microsoft.EntityFrameworkCore;

namespace eUNI_API.classes;

public class AppDbContext: DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { } 
    public DbSet<TestItem> TestItem { get; set; }
}