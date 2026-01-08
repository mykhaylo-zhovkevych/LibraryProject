using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class NonExistingItemException : Exception
    {
        public NonExistingItemException(string? message) : base(message) { }
        public NonExistingItemException() : this("Apology, but no Item was found.") { }

    }
}
