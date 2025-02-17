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

    public async Task<VehicleBranch> GetVehicleBranch(int id)
    {
        var vehicleBranch = await _context.FindAsync<VehicleBranch>(id);
        if (vehicleBranch == null){
            throw new KeyNotFoundException($"Vehicle Branch with ID {id} not found.");
        }
        return vehicleBranch;
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

    public async Task CreateVehicleBranch(VehicleBranch vehicleBranch)
    {
        await _context.VehicleBranches.AddAsync(vehicleBranch);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteVehicleBranch(int id)
    {
        var vbToDelete = await _context.VehicleBranches.FindAsync(id);
        if (vbToDelete == null){
            throw new KeyNotFoundException($"Vehicle Branch with ID {id} not found.");
        }
        _context.VehicleBranches.Remove(vbToDelete);
        await _context.SaveChangesAsync();
    }
}