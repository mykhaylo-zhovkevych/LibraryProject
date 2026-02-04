using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interfaces
{
    public interface IPolicyRepository
    {
        Task SavePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default);
        Task UpdatePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default);
        Task RemovePolicyAsync(UserType userType, ItemType itemType, string policyName, CancellationToken ct = default);
        Task<Policy?> GetPolicyAsync(UserType userType, ItemType itemType, CancellationToken ct = default);
    }
}
