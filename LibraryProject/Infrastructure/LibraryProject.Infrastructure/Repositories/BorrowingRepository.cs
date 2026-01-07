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

        //public (bool Success, string Message) CreateBorrowing(User user, Item item)
        //{
        //    throw new NotImplementedException();
        //}

        public List<Borrowing> GetActiveBorrowings(Guid userId)
        {
            return _storage.Borrowings
                .Where(b => b.User.Id == userId && !b.IsReturned)
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

        public void RemoveBorrowingFromStorage(Borrowing borrowing)
        {
            throw new NotImplementedException();
        }

        public void SaveBorrowingToStorage(Borrowing borrowing)
        {
            throw new NotImplementedException();
        }
    }
}
