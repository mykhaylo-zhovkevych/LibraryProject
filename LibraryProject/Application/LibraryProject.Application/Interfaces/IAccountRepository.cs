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
        void SaveAccountToStorage(Account account);
        void DeleteAccountFromStorage(Account account);
        Account? GetAccountByAccountId(int accountId);
        Account? GetAccountByUsername(string userName);
    }
}
