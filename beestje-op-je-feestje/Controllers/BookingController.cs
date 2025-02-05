using beestje_op_je_feestje.DAL;
using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.Models.Validation;
using beestje_op_je_feestje.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace beestje_op_je_feestje.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingRepo _bookingRepo;
        private readonly AnimalRepo _animalRepo;
        private readonly AccountRepo _accountRepo;
        private readonly RestrictedAnimalValidation _restrictedAnimalValidation;
        private readonly DateRestriction _dateRestriction;
        private readonly FlyingValidation _flyingValidation;

        public BookingController(BookingRepo bookingRepository, AnimalRepo animalRepository, AccountRepo accountRepository, FlyingValidation flyingValidation, RestrictedAnimalValidation restrictedAnimalValidation, DateRestriction dateRestriction)
        {
            _bookingRepo = bookingRepository;
            _animalRepo = animalRepository;
            _accountRepo = accountRepository;
            _dateRestriction = dateRestriction;
            _restrictedAnimalValidation = restrictedAnimalValidation;
            _flyingValidation = flyingValidation;
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

                await _bookingRepo.InsertNewBookingAsync(booking);
            }
            return RedirectToAction("BookingAnimalOverview", new { selectedDate = model.SelectedDate });
        }

        [HttpGet]
        public IActionResult MyBookings()
        {
            var userEmail = User.Identity?.Name;
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login", "Account");
            }

            var userAccount = _accountRepo.GetAccountByEmail(userEmail);
            if (userAccount == null)
            {
                return NotFound("User account not found.");
            }

            var bookings = _bookingRepo.GetAllBookings()
                                      .Where(b => b.UserId == userAccount.Id)
                                      .ToList();
            var animals = _animalRepo.GetAllAnimals()
                                     .Where(a => bookings.Any(b => b.Id == a.BookingId))
                                     .ToList();

            List<int> selectIdAnimals = animals.Select(a => a.Id).ToList(); 

            var viewModelList = bookings.Select(booking => new BookingViewModel
            {
                Id = booking.Id,
                SelectedDate = booking.SelectedDate,
                Animals = animals.Where(a => a.BookingId == booking.Id).ToList(),  
                SelectedIdAnimals = selectIdAnimals,  
                DiscountType = userAccount.DiscountType
            }).ToList();

            return View(viewModelList);
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var booking = _bookingRepo.GetBookingById(id);
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Boeking niet gevonden!";
                return RedirectToAction("Home");
            }
            _bookingRepo.DeleteBookingById(id);
            _ = _bookingRepo.SaveChangesAsync();
            TempData["SuccessMessage"] = "Boeking voor " + booking.SelectedDate.ToString("yyyy-MM-dd") + " is succesvol verwijderd!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Detail(int id)
        {
            var userEmail = User.Identity?.Name;
            var userAccount = _accountRepo.GetAccountByEmail(userEmail);

            if (userAccount == null)
            {
                return NotFound("User account not found.");
            }
            var booking = _bookingRepo.GetBookingById(id);
            if (booking == null)
            {
                return NotFound("Boeking niet gevonden.");
            }

            var animals = _animalRepo.GetAllAnimals()
                                    .Where(a => a.BookingId == booking.Id)
                                    .ToList();

            List<int> selectIdAnimals = animals.Select(a => a.Id).ToList();

            var viewModel = new ContactDetailsViewModel
            {
                First_Name = userAccount.First_Name,
                Middle_Name = userAccount.Middle_Name,
                Last_Name = userAccount.Last_Name,
                Street_Name = userAccount.Street_Name,
                Street_Number = (int)userAccount.Street_Number,
                City = userAccount.City,
                Email = userAccount.Email,
                SelectedIdAnimals = selectIdAnimals,
                SelectedDate = booking.SelectedDate,
                DiscountType = userAccount.DiscountType,
                Animals = animals,
            };

            return View(viewModel);
        }




        [HttpGet]
        public IActionResult BookingAnimalOverview(DateTime selectedDate)
        {
            if (selectedDate == DateTime.MinValue)
            {
                ModelState.AddModelError("SelectedDate", "Selecteer een datum. Dit kan de huidige dag zijn of voor in de toekomst.");
                return RedirectToPage("Home");
            }

            var booking = _bookingRepo.GetBookingByDate(selectedDate);
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
            if (model.SelectedIdAnimals == null || !model.SelectedIdAnimals.Any())
            {
                ModelState.AddModelError("", "Je moet minimaal één beestje selecteren om door te gaan.");
                model.Animals = GetAvailableAnimals(model.SelectedDate);
                return View(model);
            }

            var booking = _bookingRepo.GetBookingById(model.Id);
            if (booking == null)
            {
                ModelState.AddModelError("", $"Boeking met ID {model.Id} niet gevonden.");
                model.Animals = GetAvailableAnimals(model.SelectedDate);
                return View(model);
            }

            var selectedAnimals = await _animalRepo.GetAnimalsByIdsAsync(model.SelectedIdAnimals);
            if (!selectedAnimals.Any())
            {
                ModelState.AddModelError("", "De geselecteerde dieren zijn ongeldig.");
                model.Animals = GetAvailableAnimals(model.SelectedDate);
                return View(model);
            }

            if (!_restrictedAnimalValidation.Validate(selectedAnimals, booking))
            {
                ModelState.AddModelError("", "Je mag geen Leeuw of IJsbeer boeken samen met een Boerderijdier.");
                model.Animals = GetAvailableAnimals(model.SelectedDate);
                return View(model);
            }

            if (_dateRestriction.Validate(selectedAnimals, booking))
            {
                ModelState.AddModelError("", "De geselecteerde dier(en) kunnen niet worden geboekt op de geselecteerde datum in verband met het seizoen");
                model.Animals = GetAvailableAnimals(model.SelectedDate);
                return View(model);
            }

            if (!_flyingValidation.Validate(selectedAnimals, booking))
            {
                ModelState.AddModelError("", "We houden de lucht rustig, niet meer dan twee vliegende dieren!");
                model.Animals = GetAvailableAnimals(model.SelectedDate);
                return View(model);
            }

            HttpContext.Session.SetString("SelectedAnimals", string.Join(",", model.SelectedIdAnimals));
            HttpContext.Session.SetString("SelectedDate", model.SelectedDate.ToString("yyyy-MM-dd"));

            return RedirectToAction("FillDetails_step_2", new
            {
                bookingId = model.Id,
                selectedAnimalIds = string.Join(",", model.SelectedIdAnimals),
                selectedDate = model.SelectedDate.ToString("yyyy-MM-dd")
            });
        }


        [HttpGet]
        public async Task<IActionResult> FillDetails_step_2(string selectedAnimalIds, ContactDetailsViewModel viewmodel)
        {
            var selectedAnimals = HttpContext.Session.GetString("SelectedAnimals");

            if (string.IsNullOrEmpty(selectedAnimalIds) && !string.IsNullOrEmpty(selectedAnimals))
            {
                selectedAnimalIds = selectedAnimals;
            }

            if (string.IsNullOrEmpty(selectedAnimalIds))
            {
                ModelState.AddModelError("", "Geen dieren geselecteerd.");
                return RedirectToAction("BookingAnimalOverview");
            }

            var animalIds = selectedAnimalIds.Split(',').Select(int.Parse).ToList();
            List<Animal> animals = await _animalRepo.GetAnimalsByIdsAsync(animalIds);

            // **Store values in session**
            HttpContext.Session.SetString("First_Name", viewmodel.First_Name ?? "");
            HttpContext.Session.SetString("Middle_Name", viewmodel.Middle_Name ?? "");
            HttpContext.Session.SetString("Last_Name", viewmodel.Last_Name ?? "");
            HttpContext.Session.SetString("Email", viewmodel.Email ?? "");
            HttpContext.Session.SetString("Street_Name", viewmodel.Street_Name ?? "");
            HttpContext.Session.SetInt32("Street_Number", viewmodel.Street_Number);
            HttpContext.Session.SetString("City", viewmodel.City ?? "");

            var model = new ContactDetailsViewModel
            {
                Animals = animals,
                SelectedIdAnimals = animalIds,
                First_Name = HttpContext.Session.GetString("First_Name"),
                Middle_Name = HttpContext.Session.GetString("Middle_Name"),
                Last_Name = HttpContext.Session.GetString("Last_Name"),
                Email = HttpContext.Session.GetString("Email"),
                Street_Name = HttpContext.Session.GetString("Street_Name"),
                Street_Number = (int)HttpContext.Session.GetInt32("Street_Number"),
                City = HttpContext.Session.GetString("City"),
            };

            return View(model);
        }
        [HttpGet]
        public async Task<IActionResult> SubmitContactDetails(string email, string selectedAnimals, string selectedDate, ContactDetailsViewModel model)
        {
            selectedAnimals = HttpContext.Session.GetString("SelectedAnimals");
            selectedDate = HttpContext.Session.GetString("SelectedDate");
            model.First_Name = HttpContext.Session.GetString("First_Name");
            model.Last_Name = HttpContext.Session.GetString("Last_Name");
            model.Middle_Name = HttpContext.Session.GetString("Middle_Name");
            model.Street_Name = HttpContext.Session.GetString("Street_Name");
            model.Street_Number = (int)HttpContext.Session.GetInt32("Street_Number");
            model.City = HttpContext.Session.GetString("City");

            if (string.IsNullOrEmpty(selectedAnimals))
            {
                ModelState.AddModelError("", "Geen dieren geselecteerd.");
                return RedirectToAction("FillDetails_step_2");
            }

            if (string.IsNullOrEmpty(selectedDate))
            {
                ModelState.AddModelError("", "Datum is niet geselecteerd.");
                return RedirectToAction("FillDetails_step_2");
            }
            if (!DateTime.TryParse(selectedDate, out DateTime parsedDate))
            {
                ModelState.AddModelError("", "Ongeldige datumnotatie.");
                return RedirectToAction("FillDetails_step_2");
            }

            var isLoggedIn = User.Identity.IsAuthenticated;
            var animalIds = selectedAnimals.Split(',').Select(int.Parse).ToList();
            var animals = await _animalRepo.GetAnimalsByIdsAsync(animalIds);

            ContactDetailsViewModel viewModel = new ContactDetailsViewModel
            {
                Email = email,
                SelectedIdAnimals = animalIds,
                SelectedDate = parsedDate,
                Animals = animals,
                IsLoggedIn = isLoggedIn
            };

            if (isLoggedIn)
            {
                var userAccount = _accountRepo.GetAccountByEmail(email);
                if (userAccount != null)
                {
                    viewModel.First_Name = userAccount.First_Name;
                    viewModel.Middle_Name = userAccount.Middle_Name;
                    viewModel.Last_Name = userAccount.Last_Name;
                    viewModel.Street_Name = userAccount.Street_Name;
                    viewModel.Street_Number = (int)userAccount.Street_Number;
                    viewModel.City = userAccount.City;
                    viewModel.Email = userAccount.Email;
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitContactDetails(ContactDetailsViewModel model, bool IsLoggedIn, string selectedAnimals, string selectedDate)
        {
            selectedAnimals = HttpContext.Session.GetString("SelectedAnimals");
            selectedDate = HttpContext.Session.GetString("SelectedDate");

            // **Retrieve stored values**
            model.First_Name = HttpContext.Session.GetString("First_Name");
            model.Middle_Name = HttpContext.Session.GetString("Middle_Name");
            model.Last_Name = HttpContext.Session.GetString("Last_Name");
            model.Email = HttpContext.Session.GetString("Email");
            model.Street_Name = HttpContext.Session.GetString("Street_Name");
            model.Street_Number = HttpContext.Session.GetInt32("Street_Number") ?? 0;
            model.City = HttpContext.Session.GetString("City");

            if (string.IsNullOrEmpty(selectedAnimals))
            {
                ModelState.AddModelError("", "Geen dieren geselecteerd.");
                return View("FillDetails_Step_2", model);
            }

            var animalIds = selectedAnimals.Split(',').Select(int.Parse).ToList();
            IsLoggedIn = User.Identity.IsAuthenticated;
            Account account = null;

            if (IsLoggedIn)
            {
                var userEmail = User.Identity.Name;
                account = _accountRepo.GetAccountByEmail(userEmail);
                if (account == null)
                {
                    ModelState.AddModelError("", "Geen account gevonden voor de ingelogde gebruiker.");
                    return View("FillDetails_Step_2", model);
                }

                ModelState.Remove(nameof(model.First_Name));
                ModelState.Remove(nameof(model.Middle_Name));
                ModelState.Remove(nameof(model.Last_Name));
                ModelState.Remove(nameof(model.Street_Name));
                ModelState.Remove(nameof(model.Street_Number));
                ModelState.Remove(nameof(model.City));
                ModelState.Remove(nameof(model.Email));
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    model.Animals = await _animalRepo.GetAnimalsByIdsAsync(animalIds);
                    model.SelectedIdAnimals = animalIds;
                    return View("FillDetails_Step_2", model);
                }

                var existingAccount = _accountRepo.GetAccountByEmail(model.Email);
                if (existingAccount == null)
                {
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
                    await _accountRepo.InsertNewAccount(account);
                }
                else
                {
                    account = existingAccount;
                }
            }

            var booking = _bookingRepo.GetBookingByDate(DateTime.Parse(selectedDate));
            if (booking == null)
            {
                ModelState.AddModelError("", "Geen boeking gevonden voor de geselecteerde datum.");
                return View("FillDetails_Step_2", model);
            }

            booking.UserId = account.Id;
            booking.AmountOfAnimals = animalIds.Count;

            foreach (var animalId in animalIds)
            {
                var animal = _animalRepo.GetAnimalById(animalId);
                if (animal != null)
                {
                    animal.IsBooked = true;
                    animal.BookingDate = booking.SelectedDate;
                    await _animalRepo.UpdateAnimalAsync(animal);
                }
            }

            await _bookingRepo.SaveChangesAsync();
            TempData["SuccessMessage"] = "Boeking succesvol opgeslagen voor: " + booking.SelectedDate.ToString("yyyy-MM-dd") + "!";

            return RedirectToAction("Index", "Home");
        }


        private List<Animal> GetAvailableAnimals(DateTime selectedDate)
        {
            var selectedDateOnly = selectedDate.Date;

            var animals = _animalRepo.GetAllAnimals()
                .Where(a => (a.BookingDate == null || a.BookingDate.Value.Date != selectedDateOnly) && a.IsBooked == false)
                .ToList();

            return animals;
        }
    }
}
