using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions.Nonexistent
{
    public class NonexistentUserException : Exception
    {
        public NonexistentUserException(string? message) : base(message) { }
        public NonexistentUserException() : this("Apology, but no User was found.") { }

    }
}
