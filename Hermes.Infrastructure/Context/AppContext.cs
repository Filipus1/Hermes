using Hermes.Application.Entities;
using Microsoft.EntityFrameworkCore;

public class AppContext : DbContext
{
    public AppContext(DbContextOptions<AppContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}