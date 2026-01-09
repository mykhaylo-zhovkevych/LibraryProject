using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions.Nonexistent
{
    public class NonexistentPolicyException : Exception
    {
        public NonexistentPolicyException(string? message) : base(message) { }
        public NonexistentPolicyException() : this("Apology, but no Policy was found.") { }

    }
}
