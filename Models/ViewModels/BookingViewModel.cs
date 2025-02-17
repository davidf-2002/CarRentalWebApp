using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRentalWebApp.Models;

public class BookingViewModel
{
    // Dynamic data for dropdown
    public List<SelectListItem>? Branches { get; set; }
    public List<SelectListItem>? Vehicles { get; set; }

    // Holds data from the booking model
    public Booking Booking { get; set; }

    public int SelectedBranchId { get; set; }
    public int SelectedVehicleId { get; set; }

}