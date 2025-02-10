using System.Data.Common;
using CarRentalWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace CarRentalWebApp.Data;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options) : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<Branch> Branches { get; set; } = null!;
    public DbSet<VehicleBranch> VehicleBranches { get; set; } = null!;
    public DbSet<Booking> Bookings { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Vehicle configuration
        modelBuilder.Entity<Vehicle>()
            .Property(v => v.VehicleId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Vehicle>()      // One-Many relationship Vehicle-VehicleBranch
            .HasMany(v => v.VehicleBranches)
            .WithOne(vb => vb.vehicle)
            .HasForeignKey(v => v.BranchId);

        // Branch configuration
        modelBuilder.Entity<Branch>()
            .Property(br => br.BranchId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Branch>()      // One-Many relationship Branch-VehicleBranch
            .HasMany(br => br.VehicleBranches)
            .WithOne(vb => vb.branch)
            .HasForeignKey(br => br.BranchId);

        // Booking configuration
        modelBuilder.Entity<Booking>()
            .Property(b => b.BookingId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.CollectionVehicleBranch)
            .WithMany(vb => vb.Bookings)
            .HasForeignKey(b => b.CollectionVehicleBranchID);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.DropoffBranch)
            .WithMany(b => b.Bookings)
            .HasForeignKey(b => b.DropoffBranchId);


        // Seed data
        modelBuilder.Entity<Vehicle>().HasData(
            new Vehicle { VehicleId = 1, Make = "BMW", Model = "320d", Type = "Sedan", Year = 2017 },
            new Vehicle { VehicleId = 2, Make = "Audi", Model = "A4", Type = "Sedan", Year = 2018 },
            new Vehicle { VehicleId = 3, Make = "Tesla", Model = "Model S", Type = "Sedan", Year = 2020 },
            new Vehicle { VehicleId = 4, Make = "Toyota", Model = "Prius", Type = "Hatchback", Year = 2019 }
        );

        modelBuilder.Entity<Branch>().HasData(
            new Branch { BranchId = 1, City = "Austin", Name = "Austin Branch" },
            new Branch { BranchId = 2, City = "Dallas", Name = "Dallas Branch" },
            new Branch { BranchId = 3, City = "Houston", Name = "Houston Branch" }
        );

        modelBuilder.Entity<VehicleBranch>().HasData(
            new VehicleBranch { VehicleBranchId = 1, VehicleId = 1, BranchId = 1 },
            new VehicleBranch { VehicleBranchId = 2, VehicleId = 2, BranchId = 2 },
            new VehicleBranch { VehicleBranchId = 3, VehicleId = 3, BranchId = 3 },
            new VehicleBranch { VehicleBranchId = 4, VehicleId = 4, BranchId = 1 }
        );

        modelBuilder.Entity<Booking>().HasData(
            new Booking { BookingId = 1, CustomerName = "John Smith", CollectionVehicleBranchID = 1, DropoffBranchId = 2, StartTime = new DateTime(2025, 2, 1) },
            new Booking { BookingId = 2, CustomerName = "Matthew Johnson", CollectionVehicleBranchID = 2, DropoffBranchId = 3, StartTime = new DateTime(2025, 2, 2) },
            new Booking { BookingId = 3, CustomerName = "Harry Brown", CollectionVehicleBranchID = 3, DropoffBranchId = 1, StartTime = new DateTime(2025, 2, 3) }
        );
    }
}