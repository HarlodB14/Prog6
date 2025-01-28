using beestje_op_je_feestje.DAL;
using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.ViewModels;
using Microsoft.AspNetCore.Identity;
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

            var booking = BookingRepo.GetBookingByDate(selectedDate);
            if (booking == null)
            {
                ModelState.AddModelError("", "Geen boeking gevonden voor de geselecteerde datum.");
                return RedirectToPage("Home");
            }

            var model = new BookingViewModel
            {
                Id = booking.Id, 
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
                TempData["SelectedAnimalIds"] = string.Join(",", model.SelectedIdAnimals);
                TempData["SelectedDate"] = string.Join(",", model.SelectedDate);

                return RedirectToAction("FillDetails_step_2", new { bookingId = model.Id, selectedAnimalIds = string.Join(",", model.SelectedIdAnimals), model.SelectedDate});
            } 

            ModelState.AddModelError("", "Je moet minimaal één beestje selecteren om door te gaan.");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> FillDetails_step_2(int bookingId, string selectedAnimalIds)
        {
            var tempDataAnimalIds = TempData["SelectedAnimalIds"] as string;
            TempData.Keep("SelectedAnimalIds");


            var animalIds = selectedAnimalIds.Split(',').Select(int.Parse).ToList();

            List<Animal> animals = await AnimalRepo.GetAnimalsByIdsAsync(animalIds);
            var model = new ContactDetailsViewModel
            {
                Animals = animals,
                SelectedIdAnimals = animalIds
            };

            ModelState.Clear();

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> SubmitContactDetails(string email, string selectedIdAnimals, string selectedDate)
        {
            var isLoggedIn = User.Identity.IsAuthenticated;
            var animalIds = selectedIdAnimals.Split(',').Select(int.Parse).ToList();
            var animals = await AnimalRepo.GetAnimalsByIdsAsync(animalIds);
            var viewModel = new ContactDetailsViewModel
            {
                Email = email,
                SelectedIdAnimals = animalIds,
                SelectedDate = DateTime.Parse(selectedDate),
                Animals = animals,
                IsLoggedIn = isLoggedIn // Set IsLoggedIn based on the user's authentication status
            };

            // If the user is logged in, populate their details
            if (isLoggedIn)
            {
                var userAccount = AccountRepo.GetAccountByEmail(email);
                if (userAccount != null)
                {
                    viewModel.First_Name = userAccount.First_Name;
                    viewModel.Middle_Name = userAccount.Middle_Name;
                    viewModel.Last_Name = userAccount.Last_Name;
                    viewModel.Street_Name = userAccount.Street_Name;
                    viewModel.Street_Number = userAccount.Street_Number;
                    viewModel.City = userAccount.City;
                    viewModel.Email = userAccount.Email;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactDetails(ContactDetailsViewModel model, bool IsLoggedIn, string selectedAnimalIds, string selectedDate)
        {
            var animalIds = selectedAnimalIds.Split(',').Select(int.Parse).ToList();
            IsLoggedIn = User.Identity.IsAuthenticated;
            Account account = null;

            if (IsLoggedIn)
            {
                var userEmail = User.Identity.Name;
                account = AccountRepo.GetAccountByEmail(userEmail);
                if (account == null)
                {
                    ModelState.AddModelError("", "Geen account gevonden voor de ingelogde gebruiker.");
                    return View("FillDetails_Step_2", model);
                }

                // validatie mag weg als account bestaat
                ModelState.Remove(nameof(model.First_Name));
                ModelState.Remove(nameof(model.Middle_Name));
                ModelState.Remove(nameof(model.Last_Name));
                ModelState.Remove(nameof(model.Street_Name));
                ModelState.Remove(nameof(model.Street_Number));
                ModelState.Remove(nameof(model.City));
                ModelState.Remove(nameof(model.Email));
                ModelState.Remove(nameof(model.ImageUrl));
                ModelState.Remove(nameof(model.Animals));
                ModelState.Remove(nameof(model.SelectedIdAnimals));
            }
            else
            {
                //nieuwe account aanmaken als die nog niet bestaat
                if (!ModelState.IsValid)
                {
                    model.Animals = await AnimalRepo.GetAnimalsByIdsAsync(animalIds);
                    model.SelectedIdAnimals = animalIds;
                    return View("FillDetails_Step_2", model);
                }

                account = new Account
                {
                    First_Name = model.First_Name,
                    Middle_Name = model.Middle_Name,
                    Last_Name = model.Last_Name,
                    Street_Name = model.Street_Name,
                    Street_Number = model.Street_Number,
                    City = model.City,
                    Email = model.Email
                };

                var existingAccount = AccountRepo.GetAccountByEmail(model.Email);
                if (existingAccount == null)
                {
                    await AccountRepo.InsertNewAccount(account);
                }
                else
                {
                    account.Id = existingAccount.Id; 
                }
            }

            //boekingsgegevens bijwerken
            var booking = BookingRepo.GetBookingByDate(DateTime.Parse(selectedDate));
            if (booking == null)
            {
                ModelState.AddModelError("", "Geen boeking gevonden voor de geselecteerde datum.");
                return View("FillDetails_Step_2", model);
            }
            booking.UserId = account.Id;
            BookingRepo.UpdateBooking(booking);
            await BookingRepo.SaveChangesAsync();
            await BookingRepo.UpdateAnimalIdAsync(booking.Id, animalIds);
            //dieren bijwerken met nieuwe boeking
            foreach (var animalId in animalIds)
            {
                var animal = AnimalRepo.GetAnimalById(animalId);
                if (animal != null)
                {
                    animal.IsBooked = true;
                    await AnimalRepo.UpdateAnimalAsync(animal);
                }
            }
            await AnimalRepo.SaveChangesAsync();
            TempData["SuccessMessage"] = "Uw boeking is succesvol voltooid!";
            return RedirectToAction("Index", "Home");
        }


        private List<Animal> GetAvailableAnimals(DateTime selectedDate)
        {
            var animals = AnimalRepo.GetAllAnimals().
                Where(a => a.BookingDate != selectedDate && a.IsBooked == false).ToList();

            return animals;
        }
    }
}
