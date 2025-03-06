using Microsoft.VisualBasic;
using CarRentalWebApp.Repository;
using Microsoft.AspNetCore.Mvc;
using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CarRentalWebApp.Controllers;

public class VehicleController : Controller
{
    private readonly IVehicleRepo _repo;

    public VehicleController(IVehicleRepo repo)
    {
        _repo = repo;
    }
    
    // GET: /Vehicles
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        IEnumerable<Vehicle> vehicles = await _repo.GetVehicles();
        return View(vehicles);
    }

    // GET: /Vehicles/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        return View();
    }

    // POST: /Vehicles/Create
    [HttpPost]
    public async Task<IActionResult> Create(VehicleViewModel vehicleViewModel)
    {
        if (ModelState.IsValid)
        {
            var vehicle = new Vehicle
            {
                Make = vehicleViewModel.Make,
                Model = vehicleViewModel.Model,
                Year = vehicleViewModel.Year,
                Type = vehicleViewModel.Type
            };

            await _repo.CreateVehicle(vehicle);
            return RedirectToAction(nameof(Index));
        }
        return View(vehicleViewModel);
    }

    // GET: /Vehicles/Delete/{id}
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        Vehicle vehicle = await _repo.GetVehicle(id);
        if (vehicle == null)
        {
            return NotFound();
        }
        return View(vehicle);
    }

    // DELETE: /Vehicles/Delete/{id}
    [HttpDelete]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.DeleteVehicle(id);
        return RedirectToAction(nameof(Index));
    }
}