using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Models
{
    public class DisplayedPolicy
    {
        public string PolicyName { get; }
        public int PolicyExtensions { get; }
        public decimal PolicyLoanFees { get; }
        public int PolicyLoanPeriodInDays { get; }
        public string PolicyItemType { get; }
        public string PolicyUserType { get; }

        public DisplayedPolicy (
            string policyName,
            int policyExtensions,
            decimal policyLoanFees,
            int policyLoanPeriodInDays,
            string policyItemType,
            string policyUserType
            )
        {   PolicyName = policyName;
            PolicyExtensions = policyExtensions;
            PolicyLoanFees = policyLoanFees;
            PolicyLoanPeriodInDays = policyLoanPeriodInDays;
            PolicyItemType = policyItemType;
            PolicyUserType = policyUserType;
        }
    }
}
