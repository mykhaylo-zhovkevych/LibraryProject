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
        private readonly IAuthorizationService _authorizationService;

        public PolicyService(IPolicyRepository policyRepository, IAuthorizationService authorizationService)
        {
            _policyRepository = policyRepository;
            _authorizationService = authorizationService;
        }

        public bool AddPolicy(UserType userType, ItemType itemType, Policy policy)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = _policyRepository.GetPolicy(userType, itemType);

            if (foundPolicy != null)
            {
                return false;
            }

            _policyRepository.SavePolicy(userType, itemType, policy);
            return true;
        }

        public Policy UpdatePolicyValues(UserType userType, ItemType itemType, uint extensions, decimal loanFees, uint loanPeriodDays)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = _policyRepository.GetPolicy(userType, itemType);

            if (foundPolicy == null)
            {
                throw new NonExistingPolicyException();
            }
            foundPolicy.Extensions = extensions;
            foundPolicy.LoanFees = loanFees;
            foundPolicy.LoanPeriodInDays = loanPeriodDays;
            
            _policyRepository.SavePolicy(userType, itemType, foundPolicy);
            return foundPolicy;
        }

        public bool RemovePolicy(UserType userType, ItemType itemType)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = _policyRepository.GetPolicy(userType, itemType);

            if (foundPolicy == null)
            {
                return false;
            }

            _policyRepository.RemovePolicy(userType, itemType);
            return true;
        }

    }
}
