using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class NonExistingUserException : Exception
    {
        public NonExistingUserException(string? message) : base(message) { }
        public NonExistingUserException() : this("Apology, but no User was found.") { }

    }
}
