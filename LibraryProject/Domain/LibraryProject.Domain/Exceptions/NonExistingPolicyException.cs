using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class NonExistingPolicyException : Exception
    {
        public NonExistingPolicyException() : this("No Policy was found.")
        {
        }

        public NonExistingPolicyException(string? message) : base(message)
        {
        }
    }
}
