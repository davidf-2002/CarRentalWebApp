using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc;

public class VehicleBranchController : Controller
{
    public IVehicleBranchRepository _repo;

    public VehicleBranchController(IVehicleBranchRepository repo)
    {
        _repo = repo;
    }

    // GET: VehicleBranch/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var vehicleBranches = await _repo.GetVehicleBranches();
        return View(vehicleBranches);
    }

    // GET: VehicleBranch/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: VehicleBranch/Create
    [HttpPost]
    public async Task<IActionResult> Create(VehicleBranch vehicleBranch)
    {
        if (ModelState.IsValid)
        {
            await _repo.CreateVehicleBranch(vehicleBranch);
            return RedirectToAction(nameof(Index));
        }
        return View(vehicleBranch);
    }

    // GET: VehicleBranch/Delete/{id}
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var vehicleBranch = await _repo.GetVehicleBranch(id);
        if (vehicleBranch == null)
        {
            return NotFound();
        }
        return View(vehicleBranch);
    }

    // DELETE: VehicleBranch/DeleteConfirmed/{id}
    [HttpPost, ActionName("DeleteConfirmed")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.DeleteVehicleBranch(id);
        return RedirectToAction(nameof(Index));
    }
}