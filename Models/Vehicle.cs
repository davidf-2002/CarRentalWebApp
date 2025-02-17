using System.Buffers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarRentalWebApp.Models;

public class Vehicle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int VehicleId { get; set; }
    [Required]
    public string Make { get; set; }
    [Required]
    public string Model { get; set; }
    [Required]
    public string Type { get; set; }
    [Required]
    public int Year { get; set; }

    public List<VehicleBranch> VehicleBranches { get; set; }

    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}