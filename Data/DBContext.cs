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

        modelBuilder.Entity<Vehicle>()
            .HasMany(v => v.VehicleBranches)
            .WithOne(vb => vb.Vehicle)
            .HasForeignKey(vb => vb.VehicleId);

        // Branch configuration
        modelBuilder.Entity<Branch>()
            .Property(br => br.BranchId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Branch>()
            .HasMany(br => br.VehicleBranches)
            .WithOne(vb => vb.Branch)
            .HasForeignKey(vb => vb.BranchId);

        // VehicleBranch configuration
        modelBuilder.Entity<VehicleBranch>()
            .Property(vb => vb.IsAvailable)
            .HasDefaultValue(true);

        // Booking configuration
        modelBuilder.Entity<Booking>()
            .Property(b => b.BookingId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.Vehicle)
            .WithMany(v => v.Bookings)
            .HasForeignKey(b => b.VehicleId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.PickupBranch)
            .WithMany()
            .HasForeignKey(b => b.PickupBranchId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.DropoffBranch)
            .WithMany()
            .HasForeignKey(b => b.DropoffBranchId);

        modelBuilder.Entity<Booking>()
            .HasOne(b => b.VehicleBranch)
            .WithMany()
            .HasForeignKey(b => b.VehicleBranchId)
            .OnDelete(DeleteBehavior.Cascade);

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
            new Branch { BranchId = 3, City = "Houston", Name = "Houston Branch" },
            new Branch { BranchId = 4, City = "Phoenix", Name = "Phoenix Branch" }
        );

        modelBuilder.Entity<VehicleBranch>().HasData(
            new VehicleBranch { VehicleBranchId = 1, VehicleId = 1, BranchId = 4, Rate = 110, IsAvailable = true },
            new VehicleBranch { VehicleBranchId = 2, VehicleId = 2, BranchId = 3, Rate = 125, IsAvailable = true },
            new VehicleBranch { VehicleBranchId = 3, VehicleId = 3, BranchId = 2, Rate = 95, IsAvailable = true },
            new VehicleBranch { VehicleBranchId = 4, VehicleId = 4, BranchId = 1, Rate = 135, IsAvailable = true },
            new VehicleBranch { VehicleBranchId = 5, VehicleId = 1, BranchId = 1, Rate = 105, IsAvailable = false },
            new VehicleBranch { VehicleBranchId = 6, VehicleId = 2, BranchId = 2, Rate = 88, IsAvailable = false },
            new VehicleBranch { VehicleBranchId = 7, VehicleId = 3, BranchId = 3, Rate = 120, IsAvailable = false },
            new VehicleBranch { VehicleBranchId = 8, VehicleId = 4, BranchId = 4, Rate = 130, IsAvailable = false }
        );

        modelBuilder.Entity<Booking>().HasData(
            new Booking { BookingId = 1, CustomerName = "John Smith", VehicleId = 1, PickupBranchId = 1, DropoffBranchId = 4, VehicleBranchId = 1, StartTime = new DateTime(2025, 2, 1), EndTime = new DateTime(2025, 2, 11) },
            new Booking { BookingId = 2, CustomerName = "Matthew Johnson", VehicleId = 2, PickupBranchId = 2, DropoffBranchId = 3, VehicleBranchId = 2, StartTime = new DateTime(2025, 2, 2), EndTime = new DateTime(2025, 2, 5) },
            new Booking { BookingId = 3, CustomerName = "Harry Brown", VehicleId = 3, PickupBranchId = 3, DropoffBranchId = 2, VehicleBranchId = 3, StartTime = new DateTime(2025, 2, 3), EndTime = new DateTime(2025, 2, 6) },
            new Booking { BookingId = 4, CustomerName = "Paul Johnson", VehicleId = 4, PickupBranchId = 4, DropoffBranchId = 1, VehicleBranchId = 4, StartTime = new DateTime(2025, 2, 3), EndTime = new DateTime(2025, 2, 6) }
        );
    }
}