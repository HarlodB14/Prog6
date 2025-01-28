using beestje_op_je_feestje.DAL;
using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace beestje_op_je_feestje.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AccountRepo _accountRepo;


        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AccountRepo repository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _accountRepo = repository;
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; 
            return View();
        }
        [HttpGet]
        public IActionResult Index(AccountViewModel model)
        {
            var accounts = _accountRepo.GetAllAccounts();

            return View(accounts);
        }

        [HttpGet]
        public IActionResult Register()
        {

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Register(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if email exists in both IdentityUser and Account
                var emailExists = await _userManager.FindByEmailAsync(model.Email) != null ||
                                  _accountRepo.GetAllAccounts().Any(a => a.Email == model.Email);

                if (emailExists)
                {
                    ModelState.AddModelError("", "Een account met dit e-mailadres bestaat al.");
                    return View(model);
                }

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber
                };
                var password = PasswordGenerator();
                var identityResult = await _userManager.CreateAsync(user, password);

                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(model);
                }

                var account = new Account
                {
                    Email = model.Email, 
                    First_Name = model.First_Name,
                    Last_Name = model.Last_Name,
                    PhoneNumber = model.PhoneNumber,
                    Street_Name = model.Street_Name,
                    Street_Number = model.Street_Number,
                    City = model.City,
                    DiscountType = model.DiscountType
                };

                await _accountRepo.InsertNewAccount(account);

                TempData["SuccessMessage"] = $"Nieuwe klant met naam: {model.First_Name} {model.Last_Name} aangemaakt!";
                TempData["GeneratedPassword"] = password;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var account = _accountRepo.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }
            else
            {
                var viewModel = new AccountViewModel
                {
                    Id = account.Id,
                    First_Name = account.First_Name,
                    Last_Name = account.Last_Name,
                    Email = account.Email,
                    PhoneNumber = account.PhoneNumber,
                    Street_Name = account.Street_Name,
                    Street_Number = account.Street_Number,
                    City = account.City,
                    DiscountType = account.DiscountType
                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AccountViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var account = _accountRepo.GetAccountById(model.Id);
            if (account == null)
            {
                return NotFound();
            }

            account.Id = model.Id;
            account.First_Name = model.First_Name;
            account.Last_Name = model.Last_Name;
            account.Email = model.Email;
            account.PhoneNumber = model.PhoneNumber;
            account.Street_Name = model.Street_Name;
            account.Street_Number = model.Street_Number;
            account.City = model.City;
            account.DiscountType = model.DiscountType;

            await _accountRepo.SaveChangesAsync();

            TempData["SuccessMessage"] = "Accountgegevens zijn succesvol bijgewerkt!";

            return RedirectToAction("Index", "Account");
        }

        public async Task<IdentityUser> GetUserAsync()
        {
             return await  _signInManager.UserManager.GetUserAsync(User);
        }

        public IActionResult Detail(int id)
        {
            var account = _accountRepo.GetAccountById(id);
            if (account == null)
            {
                return NotFound();
            }

            var accountViewModel = new AccountViewModel
            {
                Id = account.Id,
                First_Name = account.First_Name,
                Last_Name = account.Last_Name,
                Email = account.Email,
                PhoneNumber = account.PhoneNumber,
                Street_Name = account.Street_Name,
                Street_Number = account.Street_Number,
                City = account.City,
                DiscountType = account.DiscountType
            };

            return View(accountViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        TempData["SuccessMessage"] = "Welkom " + user.UserName + "!";

                        // Store the email in TempData
                        TempData["UserEmail"] = model.Email;

                        // Check if the user is an admin
                        var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                        if (!isAdmin)
                        {
                            // Retrieve booking data from TempData
                            var selectedIdAnimals = TempData["SelectedAnimalIds"] as string;
                            var selectedDate = TempData["SelectedDate"] as string;

                            // Ensure TempData is kept for further requests
                            TempData.Keep("SelectedAnimalIds");
                            TempData.Keep("IsLoggedIn");
                            TempData.Keep("SelectedDate");
                            TempData.Keep("Email");

                            if (!string.IsNullOrEmpty(selectedIdAnimals) &&
                                !string.IsNullOrEmpty(selectedDate) &&
                                DateTime.TryParse(selectedDate, out _))
                            {
                                // Redirect to SubmitContactDetails with the retrieved data
                                return RedirectToAction("SubmitContactDetails", "Booking", new
                                {
                                    IsLoggedIn = true,
                                    email = model.Email,
                                    selectedIdAnimals,
                                    selectedDate
                                });
                            }
                            else
                            {
                                TempData["ErrorMessage"] = "Geen boekingsgegevens gevonden. Log opnieuw in om verder te gaan.";
                                return RedirectToAction("Index", "Home");
                            }
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Invalide inlog-poging");
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Gebruiker niet gevonden");
                }
            }
            ViewData["ReturnUrl"] = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            TempData["SuccessMessage"] = "Je bent succesvol uitgelogd.";
            return RedirectToAction("Index", "Home");
        }

        public static string PasswordGenerator()
        {
            int stringLength = 8;
            var lowercase = "abcdefghijklmnopqrstuvwxyz";
            var uppercase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var digits = "0123456789";
            var specialCharacters = "!@#$%";
            var allCharacters = lowercase + uppercase + digits + specialCharacters;

            var output = new StringBuilder();
            var random = new Random();

            //minimale eisen
            output.Append(lowercase[random.Next(lowercase.Length)]);
            output.Append(uppercase[random.Next(uppercase.Length)]);
            output.Append(digits[random.Next(digits.Length)]);
            output.Append(specialCharacters[random.Next(specialCharacters.Length)]);

            for (int i = 4; i < stringLength; i++)
            {
                output.Append(allCharacters[random.Next(allCharacters.Length)]);
            }

            return new string(output.ToString().OrderBy(c => random.Next()).ToArray());
        }
    }
}
