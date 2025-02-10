using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class Branch
{    
    [Key]
    public int BranchId { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string City { get; set; }
    
    public int VehicleBranchId { get; set; }
    public List<VehicleBranch> VehicleBranches { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}