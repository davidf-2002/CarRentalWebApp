using System.Data.Common;
using CarRentalWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRentalWebApp.Data;

public class RentalDBContext : DbContext
{
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<Branch> Branches { get; set; } = null!;
    public DbSet<VehicleBranch> VehicleBranches { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}