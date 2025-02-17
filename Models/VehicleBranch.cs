using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class VehicleBranch
{
    [Key]
    public int VehicleBranchId { get; set; }
    public int Rate { get; set; }    
    public int BranchId { get; set; }
    public Branch? Branch { get; set; } 
    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set; }
    public bool IsAvailable { get; set; }
}