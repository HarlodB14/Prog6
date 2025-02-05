using Microsoft.AspNetCore.Identity;
using beestje_op_je_feestje.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace beestje_op_je_feestje.DAL
{
    public class AccountRepo
    {
        private readonly AnimalPartyContext _animalPartyContext;
        private readonly UserManager<IdentityUser> _userManager;

        public AccountRepo(AnimalPartyContext animalPartyContext, UserManager<IdentityUser> userManager)
        {
            _animalPartyContext = animalPartyContext;
            _userManager = userManager;
        }

        public List<Account> GetAllAccounts()
        {
            var accounts = _animalPartyContext.Accounts
                .Select(a => new Account
                {
                    Id = a.Id,
                    First_Name = a.First_Name ?? "Geen voornaam",
                    Last_Name = a.Last_Name ?? "Geen Achternaam",
                    Email = a.Email ?? "Geen email",
                    PhoneNumber = a.PhoneNumber ?? "Geen Tel",
                    Street_Name = a.Street_Name ?? "Geen straatnaam",
                    Street_Number = a.Street_Number,
                    City = a.City ?? "Unknown",
                    DiscountType = a.DiscountType ?? "Geen"
                }).ToList();

            return accounts;
        }

        public async Task InsertNewAccount(Account account)
        {
            _animalPartyContext.Accounts.Add(account);
            await SaveChangesAsync();
        }

        public async Task InsertUserToIdentity(Account account, string password)
        {
            var user = new IdentityUser
            {
                UserName = account.Email,
                NormalizedUserName = account.Email,
                Email = account.Email,
                NormalizedEmail = account.Email
            };

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception("Gebruiker aanmaken is mislukt: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public Account GetAccountById(int id)
        {
            var account = _animalPartyContext.Accounts
                .Select(a => new Account
                {
                    Id = a.Id,
                    First_Name = a.First_Name ?? "Leeg",
                    Middle_Name = a.Middle_Name ?? "Leeg",
                    Last_Name = a.Last_Name ?? "Leeg",
                    Email = a.Email ?? "Leeg",
                    PhoneNumber = a.PhoneNumber ?? "Leeg",
                    Street_Name = a.Street_Name ?? "Leeg",
                    Street_Number = a.Street_Number,
                    City = a.City ?? "Leeg",
                    DiscountType = a.DiscountType ?? "Leeg"
                })
                .FirstOrDefault(a => a.Id == id);

            if (account == null)
            {
                throw new Exception($"Geen account gevonden met {id}");
            }

            return account;
        }

        public Account GetAccountByEmail(string email)
        {
            var account = _animalPartyContext.Accounts
                .Select(a => new Account
                {
                    Id = a.Id,
                    First_Name = a.First_Name ?? "Leeg",
                    Middle_Name = a.Middle_Name ?? "Leeg",
                    Last_Name = a.Last_Name ?? "Leeg",
                    Email = a.Email ?? "Leeg",
                    PhoneNumber = a.PhoneNumber ?? "Leeg",
                    Street_Name = a.Street_Name ?? "Leeg",
                    Street_Number = a.Street_Number,
                    City = a.City ?? "Leg",
                    DiscountType = a.DiscountType ?? "Leeg"
                })
                .FirstOrDefault(a => a.Email == email);

            return account; // Return null if no account is found
        }
        public async Task SaveChangesAsync()
        {
            await _animalPartyContext.SaveChangesAsync();
        }

        public string GetDiscountCard(string userEmail)
        {
            var account = _animalPartyContext.Accounts
               .Select(a => new Account
               {
                   Id = a.Id,
                   First_Name = a.First_Name ?? "Leeg",
                   Middle_Name = a.Middle_Name ?? "Leeg",
                   Last_Name = a.Last_Name ?? "Leeg",
                   Email = a.Email ?? "Leeg",
                   PhoneNumber = a.PhoneNumber ?? "Leeg",
                   Street_Name = a.Street_Name ?? "Leeg",
                   Street_Number = a.Street_Number,
                   City = a.City ?? "Leg",
                   DiscountType = a.DiscountType ?? "Leeg"
               })
               .FirstOrDefault(a => a.Email == userEmail);

            return account.DiscountType;
        }
    }
}