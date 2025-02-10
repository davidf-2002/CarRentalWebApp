using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class VehicleBranch
{
    [Key]
    public int VehicleBranchId { get; set; }
    public int Rate { get; set; }    
    public int BranchId { get; set; }
    public Branch branch { get; set; } 
    public int VehicleId { get; set; }
    public Vehicle vehicle { get; set; }
    public bool IsAvailable { get; set; }
    
    public List<Booking> Bookings { get; set; }
}