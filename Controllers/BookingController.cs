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
        var vehicles = _repo.GetAllVehicleBranches()
                             .Where(vb => vb.IsAvailable)
                             .Select(vb => new { vb.VehicleId, vb.vehicle.Model })
                             .Distinct()
                             .ToList();

        ViewBag.Branches = branches;
        ViewBag.Vehicles = vehicles;

        return View();
    }

    // POST: /Booking/Create
    [HttpPost]
    public IActionResult Create(Booking booking)
    {
        if (ModelState.IsValid)
        {
            var pickupBranch = _repo.GetBranchById(booking.CollectionVehicleBranchID);
            var dropoffBranch = _repo.GetBranchById(booking.DropoffBranchId);

            if (pickupBranch == null || dropoffBranch == null)
            {
                return NotFound("Pickup or Dropoff branch not found.");
            }

            _repo.AddBooking(booking);  // Add the booking using the fake repository

            return RedirectToAction("Details", new { id = booking.BookingId });
        }

        return View(booking);
    }
}
