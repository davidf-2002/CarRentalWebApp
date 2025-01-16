using System.Buffers;
using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class Vehicle
{
    public Vehicle()
    {
        FuelType = string.Empty;
        Make = string.Empty;
        Model = string.Empty;
        Type = string.Empty;
        owner = new Owner();
        VehicleBranches = new List<VehicleBranch>();
    }

    [Key]
    public int VehicleId { get; set; }
    public string FuelType { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Type { get; set; }
    public int Year { get; set; }
    public Owner owner { get; set; }
    public int OwnerId { get; set; }
    public List<VehicleBranch> VehicleBranches { get; set; }
}