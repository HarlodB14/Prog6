using beestje_op_je_feestje.Models;
using Microsoft.EntityFrameworkCore;

namespace beestje_op_je_feestje.DAL
{
    public class BookingRepo
    {
        private AnimalPartyContext _animalPartyContext;

        public BookingRepo(AnimalPartyContext context)
        {
            _animalPartyContext = context;
        }

        public async Task InsertNewBookingAsync(Booking booking)
        {
            _animalPartyContext.Add(booking);
            await SaveChangesAsync();
        }

        public List<Booking> GetAllBookings()
        {
            var bookings = _animalPartyContext.Bookings.ToList();

            return bookings;
        }

        public Booking GetBookingById(int id)
        {
            var booking = _animalPartyContext.Bookings.FirstOrDefault(a => a.Id == id);

            return booking;
        }

        public void DeleteBookingById(int id)
        {
            var booking = GetBookingById(id);
            _animalPartyContext.Bookings.Remove(booking);
            _animalPartyContext.SaveChanges();

            if (!_animalPartyContext.Animals.Any())
            {
                _animalPartyContext.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('bookings', RESEED, 0)");
            }
        }

        public async Task SaveChangesAsync()
        {
            await _animalPartyContext.SaveChangesAsync();
        }

    }
}