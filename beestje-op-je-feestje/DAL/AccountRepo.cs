using Microsoft.AspNetCore.Identity;
using beestje_op_je_feestje.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            List<Account> accounts = _animalPartyContext.Accounts.ToList();
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

            var result = await _userManager.CreateAsync(user,password);

            if (!result.Succeeded)
            {
                throw new Exception("Gebruiker aanmaken is mislukt: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public Account GetAccountById(int id)
        {
            var account = _animalPartyContext.Accounts.FirstOrDefault(a => a.Id == id);
            return account;
        }

        public async Task SaveChangesAsync()
        {
            await _animalPartyContext.SaveChangesAsync();
        }
    }
}