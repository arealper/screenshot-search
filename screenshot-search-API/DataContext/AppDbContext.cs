using Microsoft.EntityFrameworkCore;
using ScreenshotSearchApi.Models;

namespace ScreenshotSearchApi.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Screenshot> Screenshots => Set<Screenshot>();
}
