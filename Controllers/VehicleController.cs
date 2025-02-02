using Microsoft.VisualBasic;
using CarRentalWebApp.Repository;
using Microsoft.AspNetCore.Mvc;
using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CarRentalWebApp.Controllers;

public class VehicleController : Controller
{
    private readonly IVehicleRepo _repo;

    public VehicleController(IVehicleRepo repo)
    {
        _repo = repo;
    }
    
    // Get: /Vehicles
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<Vehicle> vehicles = await _repo.GetVehicles();
        return View(vehicles);
    }

}