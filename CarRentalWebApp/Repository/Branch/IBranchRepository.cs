using CarRentalWebApp.Models;

public interface IBranchRepository
{
    public Task<IEnumerable<Branch>> GetBranches();
    public Task<Branch> GetBranch(int id);
    public Task CreateBranch(Branch branch);
    public Task DeleteBranch(int it);
}