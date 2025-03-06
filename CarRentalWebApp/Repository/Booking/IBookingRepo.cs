using CarRentalWebApp.Models;

namespace CarRentalWebApp.Repository
{
    public interface IBookingRepo
    {
        Task<List<Booking>> GetAllBookings();
        Task<Booking> GetBookingById(int id);
        Task CreateBooking(Booking booking);
        Task UpdateBooking(Booking booking);
        Task DeleteBooking(int bookingId);
    }
}