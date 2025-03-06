using CarRentalWebApp.Models;

public class HomeViewModel
{
    public IEnumerable<Vehicle>? Vehicles { get; set; }
    public IEnumerable<Branch>? Branches { get; set; }
}