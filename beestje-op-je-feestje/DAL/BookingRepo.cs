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

        public async Task UpdateAnimalIdAsync(int bookingId, List<int> animalIds)
        {
            var booking = await _animalPartyContext.Bookings
                .Include(b => b.Animals) // Load the Animals collection
                .FirstOrDefaultAsync(b => b.Id == bookingId) ?? throw new ArgumentException($"Boeking met ID {bookingId} niet gevonden.");

            var validAnimals = await _animalPartyContext.Animals
                .Where(a => animalIds.Contains(a.Id))
                .ToListAsync();

            if (validAnimals.Count != animalIds.Count)
            {
                throw new ArgumentException("Een of meer dieren-ID's zijn ongeldig.");
            }

            booking.Animals.Clear();

            foreach (var animal in validAnimals)
            {
                booking.Animals.Add(animal);
            }

            _animalPartyContext.Bookings.Update(booking);
            await _animalPartyContext.SaveChangesAsync();
        }

        public Booking GetBookingByDate(DateTime selectedDate)
        {
            var booking = _animalPartyContext.Bookings
                .FirstOrDefault(b => b.SelectedDate.Date == selectedDate.Date);
            return booking;
        }
    }

}