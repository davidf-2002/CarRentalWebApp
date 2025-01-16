using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class Branch
{
    public Branch()
    {
        City = string.Empty;
        Country = string.Empty;
        State = string.Empty;
        VehicleBranches = new List<VehicleBranch>();
    }
    
    [Key]
    public int BranchId { get; set; }
    public string City { get; set; }
    public string Country { get; set; }
    public float Lattitude { get; set; }
    public float Longitude { get; set; }
    public string State { get; set; }
    public List<VehicleBranch> VehicleBranches { get; set; }
}