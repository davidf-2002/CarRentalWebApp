using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

public class VehicleBranchViewModel
{
    public int Rate { get; set; }    
    public int BranchId { get; set; }
    public int VehicleId { get; set; }

    // Dynamic data for dropdown
    public List<SelectListItem>? Branches { get; set; }
    public List<SelectListItem>? Vehicles { get; set; }

    public int SelectedBranchId { get; set; }
    public int SelectedVehicleId { get; set; }
}