using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRentalWebApp.Models;

public class VehicleViewModel()
{
    public string Make { get; set; }
    public string Model { get; set; }
    public string Type { get; set; }
    public int Year { get; set; }
}