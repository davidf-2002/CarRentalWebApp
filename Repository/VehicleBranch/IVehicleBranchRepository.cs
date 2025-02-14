using CarRentalWebApp.Models;

public interface IVehicleBranchRepository
{
    public Task<IEnumerable<VehicleBranch>> GetVehicleBranches();
    public Task UpdateVehicleBranch(VehicleBranch vehicleBranch);
    public Task AddVehicleBranch(VehicleBranch vehicleBranch);
}