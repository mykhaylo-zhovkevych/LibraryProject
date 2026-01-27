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

        public RemAccountRepository(LibraryStorage storage) => _storage = storage;

        public Task UpdateAccountAsync(Account account, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetAccountByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Account>> GetAllAccountsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveAccountAsync(Account account, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAccountAsync(Account account, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetAccountByAccountIdAsync(int accountId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Account?> GetAccountByUsernameAsync(string userName, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
