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
        _context.VehicleBranches.Update(vehicleBranch);
        await _context.SaveChangesAsync();
    }

    public async Task AddVehicleBranch(VehicleBranch vehicleBranch)
    {
        await _context.VehicleBranches.AddAsync(vehicleBranch);
        await _context.SaveChangesAsync();
    }
}