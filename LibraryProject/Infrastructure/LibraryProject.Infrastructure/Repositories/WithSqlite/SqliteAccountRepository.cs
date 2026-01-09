using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteAccountRepository : IAccountRepository
    {
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

        public Task SaveAccountAsync(Account account, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
