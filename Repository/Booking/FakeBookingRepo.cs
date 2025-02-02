using System;
using System.Collections.Generic;
using System.Linq;
using CarRentalWebApp.Models;
using CarRentalWebApp.Repository;

namespace CarRentalWebApp.Repository;

public class FakeBookingRepo : IBookingRepo
{
    // Mock data storage
    public List<Booking> Bookings { get; set; }
    public List<Vehicle> Vehicles { get; set; }
    public List<Branch> Branches { get; set; }
    public List<VehicleBranch> VehicleBranches { get; set; }

    public FakeBookingRepo()
    {
        // Initialize mock data
        Vehicles = new List<Vehicle>
        {
            new Vehicle { VehicleId = 1, Model = "Ford Mustang", Make = "Ford", Year = 2020 },
            new Vehicle { VehicleId = 2, Model = "Chevrolet Camaro", Make = "Chevrolet", Year = 2021 },
            new Vehicle { VehicleId = 3, Model = "Tesla Model 3", Make = "Tesla", Year = 2022 }
        };

        Branches = new List<Branch>
        {
            new Branch { BranchId = 1, Name = "NY Branch", City = "New York" },
            new Branch { BranchId = 2, Name = "LA Branch", City = "Los Angeles" },
            new Branch { BranchId = 3, Name = "Chicago Branch", City = "Chicago" }
        };

        VehicleBranches = new List<VehicleBranch>
        {
            new VehicleBranch { VehicleBranchId = 1, VehicleId = 1, BranchId = 1, IsAvailable = true },
            new VehicleBranch { VehicleBranchId = 2, VehicleId = 1, BranchId = 2, IsAvailable = true },
            new VehicleBranch { VehicleBranchId = 3, VehicleId = 2, BranchId = 2, IsAvailable = true },
            new VehicleBranch { VehicleBranchId = 4, VehicleId = 3, BranchId = 3, IsAvailable = true }
        };

        Bookings = new List<Booking>
        {
            new Booking { BookingId = 1, StartTime = DateTime.Now, EndTime = DateTime.Now.AddHours(2), CustomerName = "John Doe", CollectionVehicleBranchID = 1, DropoffBranchId = 2 },
            new Booking { BookingId = 2, StartTime = DateTime.Now.AddHours(1), EndTime = DateTime.Now.AddHours(3), CustomerName = "Jane Smith", CollectionVehicleBranchID = 2, DropoffBranchId = 3 }
        };
    }

    public Task<List<Booking>> GetAllBookings()
    {
        return Task.FromResult((List<Booking>)Bookings);
    }

    public Task<Booking> GetBookingById(int id)
    {
        var booking = Bookings.FirstOrDefault(b => b.BookingId == id);
        return Task.FromResult(booking);
    }

    public Task AddBooking(Booking booking)
    {
        booking.BookingId = Bookings.Max(b => b.BookingId) + 1;  // Auto-increment BookingId
        Bookings.Add(booking);
        return Task.CompletedTask;
    }

    public Task UpdateBooking(Booking booking)
    {
        var existingBooking = Bookings.FirstOrDefault(b => b.BookingId == booking.BookingId);
        if (existingBooking != null)
        {
            existingBooking.StartTime = booking.StartTime;
            existingBooking.EndTime = booking.EndTime;
            existingBooking.CustomerName = booking.CustomerName;
            existingBooking.CollectionVehicleBranchID = booking.CollectionVehicleBranchID;
            existingBooking.DropoffBranchId = booking.DropoffBranchId;
        }
        return Task.CompletedTask;
    }

    public Task DeleteBooking(int bookingId)
    {
        var booking = Bookings.FirstOrDefault(b => b.BookingId == bookingId);
        if (booking != null)
        {
            Bookings.Remove(booking);
        }
        return Task.CompletedTask;
    }

    // CRUD Methods for Branch
    public IEnumerable<Branch> GetAllBranches()
    {
        return Branches;
    }

    public Branch GetBranchById(int id)
    {
        return Branches.FirstOrDefault(b => b.BranchId == id);
    }

    // CRUD Methods for VehicleBranch
    public IEnumerable<VehicleBranch> GetAllVehicleBranches()
    {
        return VehicleBranches;
    }

    public VehicleBranch GetVehicleBranchById(int id)
    {
        return VehicleBranches.FirstOrDefault(vb => vb.VehicleBranchId == id);
    }
}