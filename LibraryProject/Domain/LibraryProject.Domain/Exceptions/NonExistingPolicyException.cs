using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class NonExistingPolicyException : Exception
    {
        public NonExistingPolicyException(string? message) : base(message) { }
        public NonExistingPolicyException() : this("Apology, but no Policy was found.") { }

    }
}
