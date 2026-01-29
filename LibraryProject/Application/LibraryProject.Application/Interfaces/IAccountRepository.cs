using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interfaces
{
    public interface IAccountRepository
    {
        Task SaveAccountAsync(Account account, CancellationToken ct = default);
        Task DeleteAccountAsync(Account account, CancellationToken ct = default);
        Task UpdateAccountAsync(Account account, CancellationToken ct = default);
        Task<Account?> GetAccountByAccountIdAsync(int accountId, CancellationToken ct = default);
        Task<Account?> GetAccountByUsernameAsync(string userName, CancellationToken ct = default);

        Task<Account?> GetAccountByUserIdAsync(Guid userId, CancellationToken ct = default);
        Task<List<Account>> GetAllAccountsAsync(CancellationToken ct = default);
    }
}
