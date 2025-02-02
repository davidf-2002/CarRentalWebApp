using System.Security.Cryptography.X509Certificates;
using CarRentalWebApp.Models;

namespace CarRentalWebApp.Repository;

public class FakeVehicleRepo : IVehicleRepo
{
    private IEnumerable<Vehicle> vehicles = new List<Vehicle>
    {
        new Vehicle { VehicleId = 1, Make = "BMW", Model = "320d", Type = "Sedan", Year = 2017 },
        new Vehicle { VehicleId = 2, Make = "Audi", Model = "A4", Type = "Sedan", Year = 2018 },
        new Vehicle { VehicleId = 3, Make = "Tesla", Model = "Model S", Type = "Sedan", Year = 2020 },
        new Vehicle { VehicleId = 4, Make = "Toyota", Model = "Prius", Type = "Hatchback", Year = 2019 }
    };

    public async Task<Vehicle> GetVehicle(int id)
    {
        var vehicle = vehicles.FirstOrDefault(v => v.VehicleId == id);
        if (vehicle == null)
        {
            throw new KeyNotFoundException($"Vehicle with ID {id} not found.");
        }
        return await Task.FromResult(vehicle);
    }

    public async Task<IEnumerable<Vehicle>> GetVehicles()
    {
        return await Task.FromResult(vehicles);
    }

    public Task UpdateVehicle(Vehicle vehicle)
    {
        throw new NotImplementedException();
    }

    public Task DeleteVehicle(int id)
    {
        throw new NotImplementedException();
    }
}
