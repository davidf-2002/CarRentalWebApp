using CarRentalWebApp.Models;

public class BookingDetailsDTO
{
    public int BookingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string VehicleModel { get; set; }
    public string VehicleMake { get; set; }
    public int VehicleYear { get; set; }
    public Branch PickupLocation { get; set; }   // Pickup branch location
    public Branch? DropoffLocation { get; set; }  // Drop-off branch location
    public string CustomerName { get; set; }
}