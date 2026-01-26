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
        private (UserType, ItemType) _key;

        public RemPolicyRepository(LibraryStorage storage) => _storage = storage;


        public Task<Policy?> GetPolicyAsync(UserType userType, ItemType itemType, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _key = (userType, itemType);
            _storage.Policies.TryGetValue(_key, out Policy? policy);
            return Task.FromResult(policy);
        }

        //public Task RemovePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        //{
        //    ct.ThrowIfCancellationRequested();
        //    _key = (userType, itemType);
        //    _storage.Policies.Remove(_key);
        //    return Task.CompletedTask;
        //}

        public Task SavePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _key = (userType, itemType);
            _storage.Policies.Add(_key, policy);
            return Task.CompletedTask;
        }
        public Task<List<Policy>> GetAllPolicies(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemovePolicyAsync(UserType userType, ItemType itemType, string policyName, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
