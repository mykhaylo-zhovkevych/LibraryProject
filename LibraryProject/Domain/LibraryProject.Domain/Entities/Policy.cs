using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Policy
    {
        public Guid Id { get; init; }
        public string? PolicyName { get; init; }
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

        // For testing reason: changed to public, must be internal per default 
        // TODO:Think better way to handle 
        public void SetValues(uint extensions, decimal loanFees, uint loanPeriodInDays)
        {
            if (extensions < 0) throw new ArgumentOutOfRangeException(nameof(extensions));
            if (loanFees < 0) throw new ArgumentOutOfRangeException(nameof(loanFees));

            Extensions = extensions;
            LoanFees = loanFees;
            LoanPeriodInDays = loanPeriodInDays;
        }
    }
}
