using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
using LibraryProject.Domain.Exceptions.Nonexistent;
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

        public async Task AddPolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = await _policyRepository.GetPolicyAsync(userType, itemType, ct);

            if (foundPolicy != null)
            {
                throw new PolicyUsedByException(foundPolicy);
            }

            await _policyRepository.SavePolicyAsync(userType, itemType, policy, ct);
        }

        public async Task UpdatePolicyValuesAsync(UserType userType, ItemType itemType, uint extensions, decimal loanFees, uint loanPeriodDays, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = await _policyRepository.GetPolicyAsync(userType, itemType, ct);

            if (foundPolicy == null)
            {
                throw new NonexistentPolicyException();
            }
            foundPolicy.Extensions = extensions;
            foundPolicy.LoanFees = loanFees;
            foundPolicy.LoanPeriodInDays = loanPeriodDays;

            await _policyRepository.UpdatePolicyAsync(userType, itemType, foundPolicy, ct);
        }

        public async Task RemovePolicyAsync(UserType userType, ItemType itemType, string policyName, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = await _policyRepository.GetPolicyAsync(userType, itemType, ct);

            if (foundPolicy == null)
            {
                throw new NonexistentPolicyException();
            }

            await _policyRepository.RemovePolicyAsync(userType, itemType, policyName);
        }

        // One UserType and ItemType can have only one Policy
        public async Task<List<(UserType, ItemType, Policy)>> ReceiveExistingPoliciesWithTypesAsync(CancellationToken ct)
        {
            List<(UserType, ItemType, Policy)> policies = new List<(UserType, ItemType, Policy)>();

            foreach (UserType userType in Enum.GetValues(typeof(UserType)))
            {
                foreach (ItemType itemType in Enum.GetValues(typeof(ItemType)))
                {
                    Policy? policy = await _policyRepository.GetPolicyAsync(userType, itemType, ct);
                    if ( policy == null)
                    {
                        continue;
                    }
                    policies.Add((userType, itemType, policy) );
                }
            }
            return policies;
        }
    }
}
