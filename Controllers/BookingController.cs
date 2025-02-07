using Microsoft.AspNetCore.Mvc;
using CarRentalWebApp.Repository;
using CarRentalWebApp.Models;

public class BookingController : Controller
{
    private readonly IBookingRepo _repo;

    public BookingController(IBookingRepo repo)
    {
        _repo = repo;
    }

    // GET: /Booking/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var bookings = await _repo.GetAllBookings();
        return View(bookings);
    }

    // GET: /Booking/Create
    public IActionResult Create()
    {
        var branches = _repo.GetAllBranches();
        ViewBag.Branches = branches.Select(b => new { b.BranchId, b.Name }).ToList();
        ViewBag.Vehicles = new List<object>(); // Initialize with an empty list

        // Debugging statement
        Console.WriteLine("Branches:");
        foreach (var branch in ViewBag.Branches)
        {
            Console.WriteLine($"BranchId: {branch.BranchId}, Name: {branch.Name}");
        }

        return View();
    }

    // POST: /Booking/Create
    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            Console.WriteLine($"Selected Branch ID: {booking.CollectionVehicleBranchID}");

            // Retrieve the corresponding VehicleBranch using CollectionVehicleBranchID
            var vehicleBranch = _repo.GetAllVehicleBranches()
                                    .FirstOrDefault(vb => vb.BranchId == booking.CollectionVehicleBranchID && vb.VehicleId == booking.VehicleId);

            if (vehicleBranch == null)
            {
                return NotFound("Vehicle Branch not found.");
            }

            // Set the CollectionBranch property to the retrieved VehicleBranch
            booking.CollectionBranch = vehicleBranch;


            _repo.AddBooking(booking);  // Add the booking using the fake repository

            return RedirectToAction("Details", new { id = booking.BookingId });
        }
        
        // Log ModelState errors
        foreach (var state in ModelState)
        {
            foreach (var error in state.Value.Errors)
            {
                Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
            }
        }

        // Repopulate ViewBag properties when returning the view after a failed form submission
        var branches = _repo.GetAllBranches();
        ViewBag.Branches = branches.Select(b => new { b.BranchId, b.Name }).ToList();
        ViewBag.Vehicles = new List<object>(); // Initialize with an empty list
        
        return View(booking);
    }

    // GET: /Booking/GetVehiclesByBranch
    [HttpGet]
    public async Task<IActionResult> GetVehiclesByBranch(int branchId)
    {
        var vehicleBranches = _repo.GetAllVehicleBranches()
                                .Where(vb => vb.BranchId == branchId && vb.IsAvailable)
                                .ToList();

        var vehicles = new List<Vehicle>();

        foreach (var vb in vehicleBranches)
        {
            var vehicle = await _repo.GetVehicleById(vb.VehicleId);
            if (vehicle != null)
            {
                vehicles.Add(vehicle);
            }
        }

        var vehicleModels = vehicles.Select(v => new { v.VehicleId, v.Model }).Distinct().ToList();
        return Json(vehicleModels);
    }
}
