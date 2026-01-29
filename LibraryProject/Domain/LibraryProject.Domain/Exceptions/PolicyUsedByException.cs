using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class PolicyUsedByException : Exception
    {
        public PolicyUsedByException(Policy policy) : base($"Apology, but {policy.PolicyName} is allready registered.") { }
    }
}
