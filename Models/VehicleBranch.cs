using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class VehicleBranch
{
    public VehicleBranch()
    {
        branch = new Branch();
        vehicle = new Vehicle();
    }

    [Key]
    public int VehicleBranchId { get; set; }
    public Branch branch { get; set; } 
    public int BranchId { get; set; }
    public Vehicle vehicle { get; set; }
    public int VehicleId { get; set; }
    public int Rate { get; set; }
}