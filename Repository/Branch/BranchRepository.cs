using CarRentalWebApp.Data;
using CarRentalWebApp.Models;
using Microsoft.EntityFrameworkCore;

public class BranchRepository : IBranchRepository
{
    public DBContext _context;

    public BranchRepository(DBContext context)
    {
        _context = context;
    }

    public async Task CreateBranch(Branch branch)
    {
        await _context.AddAsync(branch);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBranch(int id)
    {
        var BranchToDelete = await _context.Branches.FindAsync(id);
        if (BranchToDelete == null){
            throw new KeyNotFoundException($"Branch with ID {id} not found.");
        }
    }

    public async Task<Branch> GetBranch(int id)
    {
        var Branch = await _context.FindAsync<Branch>(id);
        if (Branch == null){
            throw new KeyNotFoundException($"Branch with ID {id} not found.");
        }
        return Branch;
    }

    public async Task<IEnumerable<Branch>> GetBranches()
    {
        var Branches = await _context.Branches.ToListAsync();
        return Branches;
    }

    public async Task UpdateBranch(Branch branch)
    {
        var BranchToUpdate = await _context.FindAsync<Branch>(branch.BranchId);
        if (BranchToUpdate == null){
            throw new KeyNotFoundException($"Branch with ID {branch.BranchId} not found.");
        }
        await _context.AddAsync(BranchToUpdate);
    }
}