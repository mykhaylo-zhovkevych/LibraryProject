using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithRem
{
    public class RemAccountRepository : IAccountRepository
    {
        private readonly LibraryStorage _storage;

        public RemAccountRepository(LibraryStorage storage)
        {
            _storage = storage;
        }
        public Task<Account?> GetAccountByUsernameAsync(string userName, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return Task.FromResult(_storage.Accounts.FirstOrDefault(u => u.Name == userName));

        }

        public Task<Account?> GetAccountByAccountIdAsync(int accountId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return Task.FromResult(_storage.Accounts.FirstOrDefault(u => u.AccountId == accountId));
        }

        public Task DeleteAccountAsync(Account account, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _storage.Accounts.Remove(account);
            return Task.CompletedTask;
        }

        public Task SaveAccountAsync(Account account, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _storage.Accounts.Add(account);
            return Task.CompletedTask;
        }
    }
}
