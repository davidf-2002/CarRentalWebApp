using CarRentalWebApp.Models;
using CarRentalWebApp.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;

public class BookingService
{
    private readonly IBookingRepo _bookingRepo;
    private readonly IVehicleRepo _vehicleRepo;
    private readonly IBranchRepository _branchRepository;
    private readonly IVehicleBranchRepository _vehicleBranchRepository;

    public BookingService(IBookingRepo bookingRepo, IVehicleRepo vehicleRepo, IBranchRepository branchRepository, IVehicleBranchRepository vehicleBranchRepository)
    {
        _bookingRepo = bookingRepo;
        _branchRepository = branchRepository;
        _vehicleBranchRepository = vehicleBranchRepository;
        _vehicleRepo = vehicleRepo;
    }

    public async Task CreateBooking(Booking booking)
    {
        // Create the booking
        await _bookingRepo.CreateBooking(booking);

        // Update the Vehicle Branch
        var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
        var vehicleBranch = vehicleBranches.FirstOrDefault(vb => vb.VehicleBranchId == booking.CollectionVehicleBranchID);

        if (vehicleBranch == null)
        {
            throw new InvalidOperationException($"VehicleBranch with ID {booking.CollectionVehicleBranchID} not found.");
        }
        else
        {
            // Set the CollectionVehicleBranch property to the retrieved VehicleBranch
            booking.CollectionVehicleBranch = vehicleBranch;

            // Mark the VehicleBranch as unavailable
            vehicleBranch.IsAvailable = false;
            await _vehicleBranchRepository.UpdateVehicleBranch(vehicleBranch);
        }
    }

    public async Task EndBooking(Booking booking)
    {
        
    } 

    public async Task<List<SelectListItem>> GetBranchesAsync()
    {
        var branches = await _branchRepository.GetBranches();
        return branches.Select(b => new SelectListItem
        {
            Value = b.BranchId.ToString(),
            Text = b.Name
        }).ToList();
    }
    
    public async Task<List<SelectListItem>> GetVehiclesByBranchAsync(int branchId)
    {
        var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();

        var filteredVehicleBranches = vehicleBranches.Where(vb => vb.BranchId == branchId).ToList();

        // Get associated vehicles from filtered vehicle branches
        var vehicles = new List<SelectListItem>();
        foreach (var vehicleBranch in filteredVehicleBranches)
        {
            var vehicle = await _vehicleRepo.GetVehicle(vehicleBranch.VehicleId);
            if (vehicle != null)
            {
                vehicles.Add(new SelectListItem
                {
                    Value = vehicle.VehicleId.ToString(),
                    Text = vehicle.Make + " " + vehicle.Model
                });
            }
        }
        return vehicles;
    }
}