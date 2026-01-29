using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions.Nonexistent
{
    public class NonexistingAccountException: Exception
    {
        public NonexistingAccountException(string? message) : base(message) { }
        public NonexistingAccountException() : this("Apology, but no Account was found.") { }

    }
}
