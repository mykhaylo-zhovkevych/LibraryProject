using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories
{
    public class BorrowingRepository : IBorrowingsRepository
    {
        private readonly InMemoryStorage _storage;

        public BorrowingRepository(InMemoryStorage storage)
        {
            _storage = storage;
        }

        public List<Borrowing> GetActiveBorrowings(Guid userId)
        {
            return _storage.Borrowings
                .Where(b => b.User.Id == userId && !b.IsReturned)
                .ToList();
        }

        public List<Borrowing> GetInactiveBorrowings(Guid userId)
        {
            return _storage.Borrowings
                .Where(b => b.User.Id == userId && b.IsReturned)
                .ToList();
        }

        public List<Borrowing> GetAllBorrowings(Guid userId)
        {
            return _storage.Borrowings
                .Where(b => b.User.Id == userId)
                .ToList();
        }

        public (Item?, User?) GetPossibleBorrowing(Guid userId, Guid itemId)
        {
            var user = _storage.Users
                .FirstOrDefault(u => u.Id == userId);

            var item = _storage.Shelves
                .SelectMany(s => s.Items)
                .FirstOrDefault(i => i.Id == itemId);

            return (item, user);
        }

        public void RemoveBorrowing(Borrowing borrowing)
        {
            _storage.Borrowings.Remove(borrowing);
        }

        public void SaveBorrowing(Borrowing borrowing)
        {
            _storage.Borrowings.Add(borrowing);
        }
    }
}
