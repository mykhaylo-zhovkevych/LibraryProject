using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithRem
{
    public class RemPolicyRepository : IPolicyRepository
    {
        private readonly LibraryStorage _storage;
        // private (UserType, ItemType) _key;

        public RemPolicyRepository(LibraryStorage storage) => _storage = storage;

        public Task<List<Policy>> GetAllPolicies(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Policy?> GetPolicyAsync(UserType userType, ItemType itemType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemovePolicyAsync(UserType userType, ItemType itemType, string policyName, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SavePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
