using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class Branch
{
    public Branch()
    {
        Name = string.Empty;
        City = string.Empty;
        VehicleBranches = new List<VehicleBranch>();
    }
    
    [Key]
    public int BranchId { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    
    public List<VehicleBranch> VehicleBranches { get; set; }
    public List<Booking> PickupBookings { get; set; }
    public List<Booking> DropoffBookings { get; set; }
}