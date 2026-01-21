using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class AccountUsedException : Exception
    {
        public AccountUsedException(Account account) : base($"Apology, but {account.AccountName} is allready used.") { }
    }
}
