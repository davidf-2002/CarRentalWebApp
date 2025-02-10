using Microsoft.AspNetCore.Mvc;
using CarRentalWebApp.Repository;
using CarRentalWebApp.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Drawing;
using System.Threading.Tasks;

public class BookingController : Controller
{
    private readonly BookingService _service;

    public BookingController(BookingService service)
    {
        _service = service;
    }

    // // GET: /Booking/Index
    // [HttpGet]
    // public async Task<IActionResult> Index()
    // {
    //     var bookings = await _repo.GetAllBookings();
    //     return View(bookings);
    // }


    // GET: /Booking/Create
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var viewModel = new BookingViewModel
        {
            Booking = new Booking(),
            Branches = await _service.GetBranchesAsync(),
            Vehicles = new List<SelectListItem>()   // Initially empty
        };
        return View(viewModel);
    }

    // POST: /Booking/Create
    [HttpPost]
    public async Task<IActionResult> Create(BookingViewModel viewModel)
    {
        if(ModelState.IsValid)
        {
            var booking = viewModel.Booking; // Map ViewModel back to Booking model

            await _service.CreateBooking(booking);
            
            return RedirectToAction("Index");
        }
        // If the model is invalid, reload the dropdowns and return view
        viewModel.Branches = await _service.GetBranchesAsync();
        viewModel.Vehicles = new List<SelectListItem>();
        return View(viewModel);
    }

    // GET: /Booking/GetVehiclesByBranch
    [HttpGet]
    public async Task<IActionResult> GetVehiclesByBranch(int branchId)
    {
        var vehicles = await _service.GetVehiclesByBranchAsync(branchId);
        return Json(vehicles);
    }




    // // GET: /Booking/Create
    // public IActionResult Create()
    // {
    //     var branches = _repo.GetAllBranches();
    //     ViewBag.Branches = branches.Select(b => new { b.BranchId, b.Name }).ToList();

    //     // Debugging statement
    //     Console.WriteLine("Branches:");
    //     foreach (var branch in ViewBag.Branches)
    //     {
    //         Console.WriteLine($"BranchId: {branch.BranchId}, Name: {branch.Name}");
    //     }

    //     return View();
    // }

    // // POST: /Booking/Create
    // [HttpPost]
    // public IActionResult Create(Booking booking)
    // {
    //     // Retrieve the corresponding VehicleBranch using CollectionVehicleBranchID
    //     var vehicleBranch = _repo.GetAllVehicleBranches()
    //                             .FirstOrDefault(vb => vb.BranchId == booking.CollectionVehicleBranchID);

    //     if (vehicleBranch == null)
    //     {
    //         ModelState.AddModelError("CollectionVehicleBranchID", "Vehicle Branch not found.");
    //     }
    //     else
    //     {
    //         // Set the CollectionVehicleBranch property to the retrieved VehicleBranch
    //         booking.CollectionVehicleBranch = vehicleBranch;

    //         // Mark the vehicle as unavailable
    //         vehicleBranch.IsAvailable = false;
    //         _repo.UpdateVehicleBranch(vehicleBranch);

    //         // Set the DropoffBranch if provided
    //         if (booking.DropoffBranchId.HasValue)
    //         {
    //             var dropoffBranch = _repo.GetAllBranches()
    //                                         .FirstOrDefault(b => b.BranchId == booking.DropoffBranchId.Value);
    //             if (dropoffBranch != null)
    //             {
    //                 booking.DropoffBranch = dropoffBranch;
    //             }
    //             else
    //             {
    //                 ModelState.AddModelError("DropoffBranchId", "Drop-off Branch not found.");
    //             }
    //         }
    //     }

    //     if (ModelState.IsValid)
    //     {
    //         _repo.AddBooking(booking);  // Add the booking using the repository
    //         return RedirectToAction("Details", new { id = booking.BookingId });
    //     }

    //     // Log ModelState errors
    //     foreach (var state in ModelState)
    //     {
    //         foreach (var error in state.Value.Errors)
    //         {
    //             Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
    //         }
    //     }

    //     return View(booking);
    // }



    // // GET: /Booking/GetVehiclesByBranch
    // [HttpGet]
    // public async Task<IActionResult> GetVehiclesByBranch(int branchId)
    // {
    //     var vehicleBranches = _repo.GetAllVehicleBranches()
    //                             .Where(vb => vb.BranchId == branchId && vb.IsAvailable)
    //                             .ToList();

    //     var vehicles = new List<Vehicle>();

    //     foreach (var vb in vehicleBranches)
    //     {
    //         var vehicle = await _repo.GetVehicleById(vb.VehicleId);
    //         if (vehicle != null)
    //         {
    //             vehicles.Add(vehicle);
    //         }
    //     }

    //     var vehicleModels = vehicles.Select(v => new { v.VehicleId, v.Model }).Distinct().ToList();
    //     return Json(vehicleModels);
    // }
}
