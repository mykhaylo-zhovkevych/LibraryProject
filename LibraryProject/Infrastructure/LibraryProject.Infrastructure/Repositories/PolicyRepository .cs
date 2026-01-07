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

namespace LibraryProject.Infrastructure.Repositories
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly InMemoryStorage _storage;
        private (UserType, ItemType) _key;

        public PolicyRepository(InMemoryStorage storage)
        {
            _storage = storage;
        }

        public Policy? GetPolicy(UserType userType, ItemType itemType)
        {
            _key = (userType, itemType);
            _storage.Policies.TryGetValue(_key, out Policy? policy);
            return policy;
        }

        public void RemovePolicyFromStorage(UserType userType, ItemType itemType)
        {
            _key = (userType, itemType);
            _storage.Policies.Remove(_key);
        }

        public void SavePolicyToStorage(UserType userType, ItemType itemType, Policy policy)
        {
            _key = (userType, itemType);
            _storage.Policies.Add(_key, policy);
        }
        
    }
}
