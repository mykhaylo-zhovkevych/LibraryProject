using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class IsAlreadyBorrowedException : Exception
    {
        public IsAlreadyBorrowedException(Item item) : base($"Apology, but {item.Name} is non retrievable") { }
    }
}
