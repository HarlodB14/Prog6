using beestje_op_je_feestje.Models;
using beestje_op_je_feestje.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                    UserName = model.Email,
                    NormalizedUserName = model.Email.ToUpper(),
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                };
                var identityResult = await _userManager.CreateAsync(user);

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
                return RedirectToAction("Index");
            }
            return View(model);
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
    }
}
