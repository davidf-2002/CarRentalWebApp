public class BookingDetailsDTO
{
    public int BookingId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string VehicleModel { get; set; }
    public string VehicleMake { get; set; }
    public int VehicleYear { get; set; }
    public string PickupLocation { get; set; }   // Pickup branch location
    public string DropoffLocation { get; set; }  // Drop-off branch location
    public string CustomerName { get; set; }
}