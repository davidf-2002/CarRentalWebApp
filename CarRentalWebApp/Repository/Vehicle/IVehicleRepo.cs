using CarRentalWebApp.Models;
namespace CarRentalWebApp.Repository;

public interface IVehicleRepo
{
    public Task<Vehicle> GetVehicle(int id);
    public Task<IEnumerable<Vehicle>> GetVehicles();
    public Task CreateVehicle(Vehicle vehicle);
    public Task DeleteVehicle(int id);
}