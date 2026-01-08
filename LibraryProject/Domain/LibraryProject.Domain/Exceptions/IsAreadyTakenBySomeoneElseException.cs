using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class IsAreadyTakenBySomeoneElseException : Exception
    {
        public IsAreadyTakenBySomeoneElseException(Account account) : base($"Apology, but {account.Name} is allready used.") { }
    }
}
