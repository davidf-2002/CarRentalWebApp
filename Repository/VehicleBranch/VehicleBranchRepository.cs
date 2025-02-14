using CarRentalWebApp.Data;
using CarRentalWebApp.Models;
using Microsoft.EntityFrameworkCore;

public class VehicleBranchRepository : IVehicleBranchRepository
{
    public DBContext _context;
    public VehicleBranchRepository(DBContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<VehicleBranch>> GetVehicleBranches()
    {
        var vehicleBranches = await _context.VehicleBranches.ToListAsync();
        return vehicleBranches;
    }

    public async Task UpdateVehicleBranch(VehicleBranch vehicleBranch)
    {
        // var VbToChange = await _context.VehicleBranches.FindAsync(vehicleBranch.VehicleBranchId);
        // if (VbToChange == null){
        //     throw new KeyNotFoundException($"VehicleBranch with ID {vehicleBranch.VehicleBranchId} not found.");
        // }
        _context.VehicleBranches.Update(vehicleBranch);
        await _context.SaveChangesAsync();
    }
}