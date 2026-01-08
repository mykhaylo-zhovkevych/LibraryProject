using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly InMemoryStorage _storage;

        public AccountRepository(InMemoryStorage storage)
        {
            _storage = storage;
        }
        public Account? GetAccountByUsername(string userName)
        {
            return _storage.Accounts.FirstOrDefault(u => u.Name == userName);

        }

        public Account? GetAccountByAccountId(int accountId)
        {
            return _storage.Accounts.FirstOrDefault(u => u.AccountId == accountId);

        }

        public void DeleteAccountFromStorage(Account account)
        {
            _storage.Accounts.Remove(account);
        }

        public void SaveAccountToStorage(Account account)
        {
            _storage.Accounts.Add(account);
        }

    }
}
