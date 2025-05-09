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

    // GET: /Booking/Index
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var bookings = await _service.GetAllBookings();
        return View(bookings);
    }

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
        var selectedBranchId = viewModel.SelectedBranchId;
        var selectedVehicleBranchId = viewModel.SelectedVehicleBranchId;

        viewModel.Booking.PickupBranchId = selectedBranchId;
        viewModel.Booking.VehicleBranchId = selectedVehicleBranchId; 

        var vehicleId = await _service.GetVehicleIdByVb(selectedVehicleBranchId);
        viewModel.Booking.VehicleId = vehicleId;

        if (ModelState.IsValid)
        {
            // Create Booking
            var booking = viewModel.Booking; // Map ViewModel back to Booking model
            await _service.CreateBooking(booking);

            return RedirectToAction("Index");
        }
        else
        {
            // Log model state errors for debugging
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                }
            }

            // Reload dropdowns and return view
            viewModel.Branches = await _service.GetBranchesAsync();
            viewModel.Vehicles = new List<SelectListItem>();
            return View(viewModel);
        }
    }

    // GET: /Booking/End
    [HttpGet]
    public async Task<IActionResult> End(int id)
    {
        var bookings = await _service.GetAllBookings();
        var booking = bookings.Where(b => b.BookingId == id).FirstOrDefault();
        if(booking == null)
        {
            return NotFound();
        }
        var viewModel = new BookingViewModel
        {
            Booking = booking,
            Branches = await _service.GetBranchesAsync()
        };

        return View(viewModel);
    }

    // POST: /Booking/EndConfirmed
    [HttpPost]
    public async Task<IActionResult> EndConfirmed(BookingViewModel bookingViewModel)
    {    
        if (ModelState.IsValid)
        {
            await _service.EndBooking(bookingViewModel.Booking);
            Console.WriteLine("Booking ended successfully");
            return RedirectToAction("Index");
        }        
        else
        {
            // Log the model state errors
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
                }
            }
            return View(bookingViewModel);
        }
    }

    // GET: /Booking/GetVehiclesByBranch
    [HttpGet]
    public async Task<IActionResult> GetVehiclesByBranch(int branchId)
    {
        var vehicles = await _service.GetVehiclesByBranchAsync(branchId);
        return Json(vehicles);
    }
}
