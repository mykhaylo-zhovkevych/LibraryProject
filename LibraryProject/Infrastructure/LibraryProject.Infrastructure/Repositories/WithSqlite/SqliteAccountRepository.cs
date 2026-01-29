using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteAccountRepository : IAccountRepository
    {
        private readonly LibraryDbContext _db;
        public SqliteAccountRepository(LibraryDbContext db) => _db = db;

        public async Task<Account?> GetAccountByUsernameAsync(string userName, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Accounts
                .FirstOrDefaultAsync(a => a.AccountName == userName, ct);
        }

        public async Task<Account?> GetAccountByAccountIdAsync(int accountId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == accountId, ct);
        }

        public async Task DeleteAccountAsync(Account account, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _db.Accounts.Remove(account);
            await _db.SaveChangesAsync(ct);
        }
        public async Task SaveAccountAsync(Account account, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await _db.Accounts.AddAsync(account, ct);
            await _db.SaveChangesAsync(ct);
        }

        public Task UpdateAccountAsync(Account account, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _db.Accounts.Update(account);
            return _db.SaveChangesAsync(ct);
        }

        public Task<Account?> GetAccountByUserIdAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.Accounts.FirstOrDefaultAsync(a => a.UserId == userId, ct);

        }

        public Task<List<Account>> GetAllAccountsAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.Accounts.ToListAsync(ct);
        }
    }
}
