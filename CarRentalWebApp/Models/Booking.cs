using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalWebApp.Models;

public class Booking
{
    [Key]
    public int BookingId { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    [Required]
    public string CustomerName { get; set; }

    public int VehicleId { get; set; }
    public Vehicle? Vehicle { get; set ; }

    public int PickupBranchId { get; set; }
    public Branch? PickupBranch { get; set ;}

    // Optional Drop off information
    public int? DropoffBranchId { get; set; }
    public Branch? DropoffBranch { get; set; }

    public int VehicleBranchId { get; set; }
    public VehicleBranch? VehicleBranch { get; set; }
}