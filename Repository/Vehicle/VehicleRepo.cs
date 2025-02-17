using CarRentalWebApp.Data;
using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace CarRentalWebApp.Repository;

public class VehicleRepo : IVehicleRepo
{
    public DBContext _context;

    public VehicleRepo(DBContext context)
    {
        _context = context;
    }

    public async Task<Vehicle> GetVehicle(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null)
        {
            throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
        }
        return vehicle;
    }

    public async Task<IEnumerable<Vehicle>> GetVehicles()
    {
        var vehicles = await _context.Vehicles.ToListAsync();
        return vehicles;
    }

    public async Task DeleteVehicle(int id)
    {
        var DeletingVehicle = await _context.Vehicles.FindAsync(id);
        if (DeletingVehicle == null)
        {
            throw new KeyNotFoundException($"Vehicle with ID {id} not found");
        }
        _context.Vehicles.Remove(DeletingVehicle);
        await _context.SaveChangesAsync();
    }

    public async Task CreateVehicle(Vehicle vehicle)
    {
        await _context.Vehicles.AddAsync(vehicle);
        await _context.SaveChangesAsync();
    }
}