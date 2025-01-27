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
        private readonly AccountRepo AccountRepo;

        public BookingController(BookingRepo bookingRepository, AnimalRepo animalRepository, AccountRepo accountRepository)
        {
            BookingRepo = bookingRepository;
            AnimalRepo = animalRepository;
            AccountRepo = accountRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new BookingViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Index(int Id, BookingViewModel model)
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
                    Id = Id,
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

            // Fetch the booking to retrieve its ID
            var booking = BookingRepo.GetBookingByDate(selectedDate);
            if (booking == null)
            {
                ModelState.AddModelError("", "Geen boeking gevonden voor de geselecteerde datum.");
                return RedirectToPage("Home");
            }

            var model = new BookingViewModel
            {
                Id = booking.Id, // Set the booking ID
                SelectedDate = selectedDate,
                Animals = GetAvailableAnimals(selectedDate)
            };

            return View(model);
        }


        [HttpPost]
        public async Task<IActionResult> BookingAnimalOverview(BookingViewModel model)
        {
            if (model.SelectedIdAnimals != null && model.SelectedIdAnimals.Any())
            {
                var booking = BookingRepo.GetBookingById(model.Id);
                if (booking == null)
                {
                    ModelState.AddModelError("", $"Boeking met ID {model.Id} niet gevonden.");
                    return View(model);
                }

                var selectedAnimals = await AnimalRepo.GetAnimalsByIdsAsync(model.SelectedIdAnimals);

                if (!selectedAnimals.Any())
                {
                    ModelState.AddModelError("", "De geselecteerde dieren zijn ongeldig.");
                    return View(model);
                }

                await BookingRepo.UpdateAnimalIdAsync(model.Id, selectedAnimals.Select(a => a.Id).ToList());

                return RedirectToAction("FillDetails_step_2", selectedAnimals);
            }

            ModelState.AddModelError("", "Je moet minimaal één beestje selecteren om door te gaan.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FillDetails_step_2(ContactDetailsViewModel model, int bookingId, List<int> selectedAnimals)
        {
            List<Animal> animals = await AnimalRepo.GetAnimalsByIdsAsync(selectedAnimals);
            List<int> animalIds = animals.Select(a => a.Id).ToList();
            model = new ContactDetailsViewModel
            {
                Animals = animals,  
                SelectedIdAnimals = animalIds 
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactDetails(ContactDetailsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("FillDetails_Step_2", model);
            }
            Account account = new()
            {
                First_Name = model.First_Name,
                Middle_Name = model.Middle_Name,
                Last_Name = model.Last_Name,
                Street_Name = model.Street_Name,
                Street_Number = model.Street_Number,
                City = model.City,
                Email = model.Email
            };
            await AccountRepo.InsertNewAccount(account);
            return RedirectToAction("Confirmation_Step3");
        }


        private List<Animal> GetAvailableAnimals(DateTime selectedDate)
        {
            var animals = AnimalRepo.GetAllAnimals().
                Where(a => a.BookingDate != selectedDate && a.IsBooked == false).ToList();

            return animals;
        }
    }
}
