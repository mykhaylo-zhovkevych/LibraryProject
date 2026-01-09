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

        public async Task<bool> AddPolicy(UserType userType, ItemType itemType, Policy policy, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = await _policyRepository.GetPolicyAsync(userType, itemType, ct);

            if (foundPolicy != null)
            {
                throw new PolicyUsedByException(foundPolicy);
            }

            await _policyRepository.SavePolicyAsync(userType, itemType, policy, ct);
            return true;
        }

        public async Task<Policy> UpdatePolicyValues(UserType userType, ItemType itemType, uint extensions, decimal loanFees, uint loanPeriodDays, CancellationToken ct)
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

            await _policyRepository.SavePolicyAsync(userType, itemType, foundPolicy, ct);
            return foundPolicy;
        }

        public async Task<bool> RemovePolicy(UserType userType, ItemType itemType, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            Policy? foundPolicy = await _policyRepository.GetPolicyAsync(userType, itemType, ct);

            if (foundPolicy == null)
            {
                throw new NonexistentPolicyException();
            }

            await _policyRepository.RemovePolicyAsync(userType, itemType);
            return true;
        }
    }
}
