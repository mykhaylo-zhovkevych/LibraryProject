using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithRem
{
    public class RemBorrowingRepository : IBorrowingsRepository
    {
        private readonly LibraryStorage _storage;

        public RemBorrowingRepository(LibraryStorage storage) => _storage = storage;

        public Task<List<Borrowing>> GetActiveBorrowingsAsync(Guid userId, CancellationToken ct = default) 
        {
            ct.ThrowIfCancellationRequested();
            List<Borrowing> quary = _storage.Borrowings
                .Where(b => b.User.Id == userId && !b.IsReturned)
                .ToList();

            return Task.FromResult(quary);
        }

        //public Task<Borrowing?> GetActiveBorrowingAsync(Guid userId, Guid itemId, CancellationToken ct = default)
        //{
        //    ct.ThrowIfCancellationRequested();
        //    Borrowing? quary = _storage.Borrowings
        //        .FirstOrDefault(b => b.User.Id == userId && 
        //        b.Item.Id == itemId && !b.IsReturned);
            
        //    return Task.FromResult(quary);
        //}

        public Task<List<Borrowing>> GetInactiveBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            List<Borrowing> quary = _storage.Borrowings
                .Where(b => b.User.Id == userId && b.IsReturned)
                .ToList();

            return Task.FromResult(quary);
        }

        public Task<List<Borrowing>> GetAllBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            List<Borrowing> quary = _storage.Borrowings
                .Where(b => b.User.Id == userId)
                .ToList();

            return Task.FromResult(quary);
        }

        //public (Item?, User?) GetPossibleBorrowing(Guid userId, Guid itemId)
        //{
        //    var user = _storage.Users
        //        .FirstOrDefault(u => u.Id == userId);

        //    var item = _storage.Shelves
        //        .SelectMany(s => s.Items)
        //        .FirstOrDefault(i => i.Id == itemId);

        //    return (item, user);
        //}

        public Task RemoveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _storage.Borrowings.Remove(borrowing);
            return Task.CompletedTask;
        }

        public Task SaveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _storage.Borrowings.Add(borrowing);
            return Task.CompletedTask;
        }

        public Task UpdateBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Borrowing?> GetActiveBorrowingByCopyAsync(Guid userId, Guid itemCopyId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountActiveBorrowingsForItemAsync(Guid itemId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
