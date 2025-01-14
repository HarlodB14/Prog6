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
        private AnimalPartyContext _context;


        public AccountController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AnimalPartyContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Index(AccountViewModel model)
        {
            //haal usergegvens op van account //TODO DAL laag laten doen
            var accounts = _context.Accounts.ToList();

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
                var user = new IdentityUser
                {
                    //email generen als die niet ingevuld wordt
                    UserName = string.IsNullOrEmpty(model.Email)
                    ? $"{model.First_name.ToLower()}.{model.Last_name.ToLower()}.{Guid.NewGuid().ToString().Substring(0, 8)}"
                      : model.Email,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };
                var password = PasswordGenerator();
                var identityResult = await _userManager.CreateAsync(user, password);

                //check of aanmaken is gelukt
                if (!identityResult.Succeeded)
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                //account vullen voor adresgegegevens
                var account = new Account
                {
                    Id = model.Id,
                    First_Name = model.First_name,
                    Last_Name = model.Last_name,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    Street_Name = model.Street_Name,
                    Street_Number = model.Street_Number,
                    City = model.City,
                    DiscountType = model.DiscountType
                };

                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Nieuwe klant met naam: " + model.Email + " aangemaakt!";
                TempData["GeneratedPassword"] = password;
                return RedirectToAction("Index");
            }
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            var account = _context.Accounts
                .Where(a => a.Id == id)
                .Select(a => new AccountViewModel
                {
                    First_name = a.First_Name,
                    Last_name = a.Last_Name,
                    Email = a.Email,
                    PhoneNumber = a.PhoneNumber,
                    Street_Name = a.Street_Name,
                    Street_Number = a.Street_Number,
                    City = a.City,
                    DiscountType = a.DiscountType
                })
                .FirstOrDefault();

            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
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
                        return RedirectToAction("Index", "Home");
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
