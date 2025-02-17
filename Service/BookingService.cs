using System.Threading.Tasks;
using CarRentalWebApp.Models;
using CarRentalWebApp.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;

public class BookingService
{
    private readonly IBookingRepo _bookingRepo;
    private readonly IBranchRepository _branchRepository;
    private readonly IVehicleBranchRepository _vehicleBranchRepository;
    private readonly IVehicleRepo _vehicleRepo;

    public BookingService(IBookingRepo bookingRepo, IBranchRepository branchRepository, IVehicleBranchRepository vehicleBranchRepository, IVehicleRepo vehicleRepo)
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
        
        var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
        var vehicleBranch = vehicleBranches.FirstOrDefault(vb => vb.BranchId == booking.PickupBranchId && vb.VehicleId == booking.VehicleId);

        if (vehicleBranch != null)
        {
            vehicleBranch.IsAvailable = false;
            await _vehicleBranchRepository.UpdateVehicleBranch(vehicleBranch);
        }
        else
        {
            Console.WriteLine("VehicleBranch not found.");
        }
    }

    public async Task EndBooking(Booking booking)
    {
        // Update the booking
        await _bookingRepo.UpdateBooking(booking);

        if (booking.DropoffBranchId.HasValue && booking.DropoffBranchId.Value != booking.PickupBranchId)
        {
            var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
            var oldVB = vehicleBranches.FirstOrDefault(vb => vb.BranchId == booking.PickupBranchId && vb.VehicleId == booking.VehicleId);

            var newVB = new VehicleBranch
            {
                Rate = oldVB.Rate,
                BranchId = booking.DropoffBranchId.Value,
                VehicleId = oldVB.VehicleId,
                IsAvailable = true
            };
            await _vehicleBranchRepository.CreateVehicleBranch(newVB);
        }
        else
        {
            var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
            var vehicleBranch = vehicleBranches.FirstOrDefault(vb => vb.BranchId == booking.PickupBranchId && vb.VehicleId == booking.VehicleId);
            if (vehicleBranch != null)
            {
                vehicleBranch.IsAvailable = true;
                await _vehicleBranchRepository.UpdateVehicleBranch(vehicleBranch);
            }
            else
            {
                Console.WriteLine("VehicleBranch not found.");
            }
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
        // Check in VehicleBranch
        var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
        var vehicleIDs = vehicleBranches.Where(vb => vb.BranchId == branchId && vb.IsAvailable == true) 
                                        .Select(vb => vb.VehicleId) 
                                        .Distinct()
                                        .ToList();

        var vehicles = new List<Vehicle>();
        foreach (var vehicleId in vehicleIDs)
        {
            var vehicle = await _vehicleRepo.GetVehicle(vehicleId);
            if (vehicle != null)
            {
                vehicles.Add(vehicle);
            }
        }

        // Turn individual vehicles into SelectListItem
        var list = new List<SelectListItem>();
        foreach (var vehicle in vehicles)
        {
            list.Add(new SelectListItem
            {
                Value = vehicle.VehicleId.ToString(),
                Text = vehicle.Make + " " + vehicle.Model
            });
        }
        return list;
    }

    public async Task<IEnumerable<Booking>> GetAllBookings()
    {
        var bookings = await _bookingRepo.GetAllBookings();
        return bookings;
    }

    // public async Task MoveVehicleToBranch(int vehicleId, int oldBranchId, int newBranchId)
    // {
    //     var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
    //     var vehicleBranch = vehicleBranches.FirstOrDefault(vb => vb.VehicleId == vehicleId && vb.BranchId == oldBranchId);
    //     if (vehicleBranch != null)
    //     {
    //         vehicleBranch.BranchId = newBranchId;
    //         await _vehicleBranchRepository.UpdateVehicleBranch(vehicleBranch);
    //     }
    //     else
    //     {
    //         throw new InvalidOperationException($"VehicleBranch with VehicleId {vehicleId} and BranchId {oldBranchId} not found.");
    //     }
    // }

    public async Task<bool> IsVehicleAssignedToBranch(int vehicleId, int branchId)
    {
        var vehicleBranches = await _vehicleBranchRepository.GetVehicleBranches();
        var vehicleBranch = vehicleBranches.FirstOrDefault(vb => vb.VehicleId == vehicleId && vb.BranchId == branchId);
        return vehicleBranch != null;
    }
}