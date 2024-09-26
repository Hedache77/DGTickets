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
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }

    public DbSet<Headquarter> Headquarters { get; set; }

    public DbSet<MedicineStock> MedicinesStock { get; set; }

    public DbSet<Pqr> Pqrs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<MedicineStock>().HasIndex(x => x.Name);
        modelBuilder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        modelBuilder.Entity<City>().HasIndex(x => new { x.StateId, x.Name }).IsUnique();
        modelBuilder.Entity<Headquarter>().HasIndex(x => new { x.CityId, x.Name }).IsUnique();

        DisableCascadingDelete(modelBuilder);
    }

    private void DisableCascadingDelete(ModelBuilder modelBuilder)
    {
        var relationships = modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys());
        foreach (var relationship in relationships)
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }
    }
}