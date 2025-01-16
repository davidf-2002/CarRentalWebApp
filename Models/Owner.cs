using System.ComponentModel.DataAnnotations;

namespace CarRentalWebApp.Models;

public class Owner
{
    public Owner()
    {
        Name = string.Empty;
        Email = string.Empty;
        Vehicles = new List<Vehicle>();
    }

    [Key]
    public int OwnerId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public List<Vehicle> Vehicles { get; set; }
}