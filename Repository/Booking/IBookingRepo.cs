using CarRentalWebApp.Models;

namespace CarRentalWebApp.Repository
{
    public interface IBookingRepo
    {
        Task<List<Booking>> GetAllBookings();
        Task<Booking> GetBookingById(int id);
        Task AddBooking(Booking booking);
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(int bookingId);

        public Task<Vehicle> GetVehicleById(int id);

        public Branch GetBranchById(int id);
        IEnumerable<Branch> GetAllBranches();
        IEnumerable<VehicleBranch> GetAllVehicleBranches();
    }
}