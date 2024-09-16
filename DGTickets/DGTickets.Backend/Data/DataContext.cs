using DGTickets.Shared.Entities;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Data;

public class DataContext : DbContext
{
    private readonly DbContextOptions<DataContext> _options;

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
        _options = options;
    }

    public DbSet<Country> Countries { get; set; }

    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        DisableCascadingDelete(modelBuilder);
    }

    private void DisableCascadingDelete(ModelBuilder modelBuilder)
    {
        var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var relationship in relationships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
    }
}