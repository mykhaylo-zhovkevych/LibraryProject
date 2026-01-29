using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using LibraryProject.Infrastructure.Persistence.InSqlite.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqlitePolicyRepository : IPolicyRepository
    {
        private readonly LibraryDbContext _db;
        public SqlitePolicyRepository(LibraryDbContext db) => _db = db;

        public async Task<Policy?> GetPolicyAsync(UserType userType, ItemType itemType, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            PolicyEntry? entry = await _db.PolicyEntries
                .Where(p => p.UserType == userType && p.ItemType == itemType)
                .OrderBy(p => p.PolicyName)
                .FirstOrDefaultAsync(ct);
            if (entry == null) 
            {
                return null; 
            }
            return new Policy(
                policyName: entry.PolicyName ?? "",
                extensions: entry.Extensions,
                loanFees: entry.LoanFees,
                loanPeriodInDays: entry.LoadPeriodInDays
            );
        }

        public async Task RemovePolicyAsync(UserType userType, ItemType itemType, string policyName, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            PolicyEntry? entry = await _db.PolicyEntries.FirstOrDefaultAsync(p => p.UserType == userType && p.ItemType == itemType && p.PolicyName == policyName, ct);
            if (entry == null)
            {
                return;
            }
            _db.PolicyEntries.Remove(entry);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SavePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            PolicyEntry? entry = await _db.PolicyEntries.FirstOrDefaultAsync(p => p.UserType == userType && p.ItemType == itemType && p.PolicyName == policy.PolicyName, ct);

            if (entry == null)
            {
                entry = new PolicyEntry
                {
                    Id = Guid.NewGuid(),
                    UserType = userType,
                    ItemType = itemType,
                    PolicyName = policy.PolicyName,
                    Extensions = policy.Extensions,
                    LoanFees = policy.LoanFees,
                    LoadPeriodInDays = policy.LoanPeriodInDays
                };
                await _db.PolicyEntries.AddAsync(entry, ct);
            }
            else
            {
                throw new InvalidOperationException("Cannot add Policy.");
            }

            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdatePolicyAsync(UserType userType, ItemType itemType, Policy policy, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            PolicyEntry? entry = await _db.PolicyEntries.FirstOrDefaultAsync(p => p.UserType == userType && p.ItemType == itemType && p.PolicyName == policy.PolicyName, ct);

            if (entry == null)
            {
                throw new InvalidOperationException("Policy to update does not exist.");
            }

            entry.Extensions = policy.Extensions;
            entry.LoanFees = policy.LoanFees;
            entry.LoadPeriodInDays = policy.LoanPeriodInDays;

            await _db.SaveChangesAsync(ct);
        }
    }
}
