using beestje_op_je_feestje.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace beestje_op_je_feestje.DAL
{
    public class AccountRepo
    {
        private AnimalPartyContext _animalPartyContext;

        public AccountRepo(AnimalPartyContext animalPartyContext)
        {
            _animalPartyContext = animalPartyContext;
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
