using DGTickets.Shared.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DGTickets.Backend.Data;

public class DataContext : IdentityDbContext<User>
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
    public DbSet<Module> Modules { get; set; }
    public DbSet<MedicineStock> MedicinesStock { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<HeadquarterMedicine> HeadquarterMedicines { get; set; }

    public DbSet<Ticket> Tickets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Country>().HasIndex(x => x.Name).IsUnique();
        modelBuilder.Entity<MedicineStock>().HasIndex(x => x.Name);
        modelBuilder.Entity<State>().HasIndex(x => new { x.CountryId, x.Name }).IsUnique();
        modelBuilder.Entity<City>().HasIndex(x => new { x.StateId, x.Name }).IsUnique();
        modelBuilder.Entity<Headquarter>().HasIndex(x => new { x.CityId, x.Name }).IsUnique();
        modelBuilder.Entity<Module>().HasIndex(x => new { x.HeadquarterId, x.Name }).IsUnique();
        modelBuilder.Entity<Rating>().HasIndex(x => x.Code).IsUnique();
        modelBuilder.Entity<HeadquarterMedicine>().HasIndex(x => new { x.HeadquarterId, x.MedicineId }).IsUnique();
        modelBuilder.Entity<Ticket>().HasIndex(x => new { x.Code }).IsUnique();

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