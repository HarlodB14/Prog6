using beestje_op_je_feestje.DAL;
using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace beestje_op_je_feestje.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingRepo BookingRepo;
        private readonly AnimalRepo AnimalRepo;

        public BookingController(BookingRepo bookingRepository, AnimalRepo animalRepository)
        {
            BookingRepo = bookingRepository;
            AnimalRepo = animalRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new BookingViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(BookingViewModel model)
        {
            //check voor datum, anders zetten op dag van vndaag
            if (model.SelectedDate == DateTime.MinValue || model.SelectedDate < DateTime.Now.Date)
            {
                model.SelectedDate = DateTime.Now.Date;
            }
            if (!ModelState.IsValid)
            {
                return View(model); 
            }
            else
            {
                var booking = new Booking
                {
                    SelectedDate = model.SelectedDate,
                    AmountOfAnimals = 0
                };

                await BookingRepo.InsertNewBookingAsync(booking);
            }
            return RedirectToAction("BookingAnimalOverview", new { selectedDate = model.SelectedDate });
        }






        [HttpGet]
        public IActionResult BookingAnimalOverview(DateTime selectedDate)
        {
            if (selectedDate == DateTime.MinValue)
            {
                ModelState.AddModelError("SelectedDate", "Selecteer een datum. Dit kan de huidige dag zijn of voor in de toekomst.");
                return RedirectToPage("Home");
            }

            var model = new BookingViewModel
            {
                SelectedDate = selectedDate,
                Animals = GetAvailableAnimals(selectedDate)
            };

            return View(model);
        }

        private List<Animal> GetAvailableAnimals(DateTime selectedDate)
        {
            var animals = AnimalRepo.GetAllAnimals().
                Where(a => a.BookingDate != selectedDate && a.IsBooked == false).ToList();

            return animals;
        }
    }
}
