using CarRentalWebApp.Models;

public interface IVehicleBranchRepository
{
    public Task<VehicleBranch> GetVehicleBranch(int id);
    public Task<IEnumerable<VehicleBranch>> GetVehicleBranches();
    public Task UpdateVehicleBranch(VehicleBranch vehicleBranch);
    public Task CreateVehicleBranch(VehicleBranch vehicleBranch);
    public Task DeleteVehicleBranch(int id);
}