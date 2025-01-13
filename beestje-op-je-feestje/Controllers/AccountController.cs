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

            foreach (Account account in accounts)
            {
                account.Id = model.Id;
                account.First_Name = model.First_name;
                account.Last_Name = model.Last_name;
                account.Email = model.Email;
                account.PhoneNumber = model.PhoneNumber;
                account.Street_Name = model.Street_Name;
                account.Street_Number = model.Street_Number;
                account.City = model.City;
                account.DiscountType = model.discountType;
            }

            return View(accounts);
        }


        [HttpPost]
        public async Task<IActionResult> Register(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                //usermanager nieuwe user laten aanmaken
                //waardes van viewmodel assignenen aan user
                //redirect naar index page van klanten

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
