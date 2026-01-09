using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqlitePolicyRepository : IPolicyRepository
    {
        public Task<Policy?> GetPolicyAsync(UserType userType, ItemType itemType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemovePolicyAsync(UserType userType, ItemType itemType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SavePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
