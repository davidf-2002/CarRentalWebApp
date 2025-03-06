using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CarRentalWebApp.Models;
using CarRentalWebApp.Repository; // Ensure this namespace contains IVehicleRepo
using CarRentalWebApp.Data;

namespace CarRentalWebApp.Controllers;

public class HomeController : Controller
{
    private readonly IVehicleRepo _vehicleRepo;
    private readonly IBranchRepository _branchRepository;

    public HomeController(IVehicleRepo vehicleRepo, IBranchRepository branchRepository)
    {
        _vehicleRepo = vehicleRepo;
        _branchRepository = branchRepository;
    }

    public async Task<IActionResult> Index()
    {
        var vehicles = await _vehicleRepo.GetVehicles();
        var branches = await _branchRepository.GetBranches();

        var viewModel = new HomeViewModel
        {
            Vehicles = vehicles,
            Branches = branches
        };

        return View(viewModel);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
