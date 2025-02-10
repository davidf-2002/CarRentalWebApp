using CarRentalWebApp.Data;
using CarRentalWebApp.Models;
using CarRentalWebApp.Repository;
using Microsoft.EntityFrameworkCore;

public class BookingRepo : IBookingRepo
{
    private readonly DBContext _context;

    public BookingRepo(DBContext context)
    {
        _context = context;
    }

    public async Task CreateBooking(Booking booking)
    {
        await _context.Bookings.AddAsync(booking);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteBooking(int bookingId)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {bookingId} not found.");
        }

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Booking>> GetAllBookings()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<Booking> GetBookingById(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {id} not found.");
        }
        return booking;
    }

    public async Task UpdateBooking(Booking booking)
    {
        var existingBooking = await _context.Bookings.FindAsync(booking.BookingId);
        if (existingBooking == null)
        {
            throw new KeyNotFoundException($"Booking with ID {booking.BookingId} not found.");
        }
        _context.Bookings.Update(existingBooking);
        await _context.SaveChangesAsync();
    }
}
