using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services
{
    public class PolicyService
    {
        private readonly IPolicyRepository _policyRepository;

        public PolicyService(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public bool AddPolicy(UserType userType, ItemType itemType, Policy policy)
        {
            Policy? foundPolicy = _policyRepository.GetPolicy(userType, itemType);

            if (foundPolicy != null)
            {
                return false;
            }

            _policyRepository.SavePolicyToStorage(userType, itemType, policy);
            return true;
        }

        public Policy UpdatePolicyValues(UserType userType, ItemType itemType, uint extensions, decimal loanFees, uint loanPeriodDays)
        {
            Policy? foundPolicy = _policyRepository.GetPolicy(userType, itemType);

            if (foundPolicy == null)
            {
                throw new NonExistingPolicyException();
            }
            foundPolicy.Extensions = extensions;
            foundPolicy.LoanFees = loanFees;
            foundPolicy.LoanPeriodInDays = loanPeriodDays;
            // Possible error not saving
            return foundPolicy;
        }

        public bool RemovePolicy(UserType userType, ItemType itemType)
        {
            Policy? foundPolicy = _policyRepository.GetPolicy(userType, itemType);

            if (foundPolicy == null)
            {
                return false;
            }

            _policyRepository.RemovePolicyFromStorage(userType, itemType);
            return true;
        }

  

        //public static void ClearPolicies()
        //{
        //    PolicyService.Policies.Clear();
        //}

    }
}
