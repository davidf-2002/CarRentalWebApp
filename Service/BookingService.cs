using System.Threading.Tasks;
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

    }

    public async Task EndBooking(Booking booking)
    {
        // Update the booking
        await _bookingRepo.UpdateBooking(booking);

        if (booking.DropoffBranchId.HasValue && booking.DropoffBranchId.Value != booking.PickupBranchId)
        {
            await MoveVehicleToBranch(booking.VehicleId, booking.PickupBranchId, booking.DropoffBranchId.Value);
        }
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
        var bookings = await _bookingRepo.GetAllBookings();
        var vehicles = bookings.Where(b => b.EndTime != null && b.PickupBranchId == branchId)
                                .Select(b => b.Vehicle)
                                .Distinct()
                                .ToList();

        // Turn individual vehicles into SelectListItem
        var list = new List<SelectListItem>();
        foreach (var vehicle in vehicles)
        {
            //var vehicle = await _vehicleRepo.GetVehicle(vehicleBranch.VehicleId);
            if (vehicle != null)
            {
                list.Add(new SelectListItem
                {
                    Value = vehicle.VehicleId.ToString(),
                    Text = vehicle.Make + " " + vehicle.Model
                });
            }
        }
        return list;
    }

    public async Task<IEnumerable<Booking>> GetAllBookings()
    {
        var bookings = await _bookingRepo.GetAllBookings();
        return bookings;
    }

    public async Task MoveVehicleToBranch(int vehicleId, int oldBranchId, int newBranchId)
    {
        var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
        var vehicleBranch = vehicleBranches.FirstOrDefault(vb => vb.VehicleId == vehicleId && vb.BranchId == oldBranchId);
        if (vehicleBranch != null)
        {
            vehicleBranch.BranchId = newBranchId;
            await _vehicleBranchRepository.UpdateVehicleBranch(vehicleBranch);
        }
        else
        {
            throw new InvalidOperationException($"VehicleBranch with VehicleId {vehicleId} and BranchId {oldBranchId} not found.");
        }
    }

    public async Task<bool> IsVehicleAssignedToBranch(int vehicleId, int branchId)
    {
        var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
        var vehicleBranch = vehicleBranches.FirstOrDefault(vb => vb.VehicleId == vehicleId && vb.BranchId == branchId);
        return vehicleBranch != null;
    }
}