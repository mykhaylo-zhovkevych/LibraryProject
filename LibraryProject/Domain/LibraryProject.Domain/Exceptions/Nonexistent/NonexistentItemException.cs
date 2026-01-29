using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions.Nonexistent
{
    public class NonexistentItemException : Exception
    {
        public NonexistentItemException(string? message) : base(message) { }
        public NonexistentItemException() : this("Apology, but no Item was found.") { }

    }
}
