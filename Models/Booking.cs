namespace CarRentalWebApp.Models;

public class Booking
{
    public int BookingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string CustomerName { get; set; }

    // Retrieves both Vehicle and Pick-up branch details
    public int CollectionVehicleBranchID { get; set; }
    public VehicleBranch CollectionBranch { get; set; }

    // Retrieves only the location for Drop-off branch
    public int DropoffBranchId { get; set; }
    public Branch DropoffBranch { get; set; }
}