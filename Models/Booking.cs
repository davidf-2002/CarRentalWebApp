using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class Booking
{
    [Key]
    public int BookingId { get; set; }
    [Required]
    public DateTime StartTime { get; set; }
    [Required]
    public DateTime EndTime { get; set; }
    [Required]
    public string CustomerName { get; set; }
    [Required]
    public int VehicleId { get; set; }

    // Retrieves both Vehicle and Pick-up branch details
    [Required]
    public int CollectionVehicleBranchID { get; set; }
    public VehicleBranch CollectionBranch { get; set; }

    // Retrieves only the location for Drop-off branch
    public int? DropoffBranchId { get; set; }
    public Branch DropoffBranch { get; set; }
}