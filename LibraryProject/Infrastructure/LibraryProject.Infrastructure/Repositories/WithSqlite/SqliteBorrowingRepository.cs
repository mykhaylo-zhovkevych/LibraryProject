using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteBorrowingRepository : IBorrowingsRepository
    {
        public Task<Borrowing?> GetActiveBorrowingAsync(Guid userId, Guid itemId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Borrowing>> GetActiveBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Borrowing>> GetAllBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Borrowing>> GetInactiveBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
