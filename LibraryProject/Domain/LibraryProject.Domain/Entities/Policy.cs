using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Policy
    {
        public Guid Id { get; private set; }
        public string? PolicyName { get; private set; }
        public uint Extensions { get; private set; }
        public decimal LoanFees { get; private set; }
        public uint LoanPeriodInDays { get; private set; }

        public Policy()
        {
            Id = Guid.NewGuid();
        }

        public Policy(string policyName, uint extensions, decimal loanFees, uint loanPeriodInDays)
        {
            Id = Guid.NewGuid();
            PolicyName = policyName;
            Extensions = extensions;
            LoanFees = loanFees;
            LoanPeriodInDays = loanPeriodInDays;

        }
    }
}
