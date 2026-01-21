using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteBorrowingRepository : IBorrowingsRepository
    {
        private readonly LibraryDbContext _db;
        public SqliteBorrowingRepository(LibraryDbContext db) => _db = db;

        public Task<int> CountActiveBorrowingsForItemAsync(Guid itemId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.Borrowings.CountAsync(b => b.ItemCopy.ItemId == itemId && b.ReturnDate == null, ct);
        }

        public Task<Borrowing?> GetActiveBorrowingByCopyAsync(Guid userId, Guid itemCopyId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.Borrowings
                .Include(b => b.ItemCopy).ThenInclude(c => c.Item)
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ItemCopyId == itemCopyId && b.ReturnDate == null, ct);
        }

        public Task<List<Borrowing>> GetActiveBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.Borrowings.AsNoTracking()
                .Include(b => b.ItemCopy).ThenInclude(c => c.Item)
                .Where(b => b.UserId == userId && b.ReturnDate == null)
                .ToListAsync(ct);
        }

        public Task<List<Borrowing>> GetInactiveBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.Borrowings.AsNoTracking()
                .Include(b => b.ItemCopy).ThenInclude(c => c.Item)
                .Where(b => b.UserId == userId && b.ReturnDate != null)
                .ToListAsync(ct);
        }

        public Task<List<Borrowing>> GetAllBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.Borrowings.AsNoTracking()
                .Include(b => b.ItemCopy).ThenInclude(c => c.Item)
                .Where(b => b.UserId == userId)
                .ToListAsync(ct);
        }

        public async Task RemoveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _db.Borrowings.Remove(borrowing);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SaveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
        // Makt the navigation entities as existing so EF won't Insert them
        //_db.Attach(borrowing.User);
        //_db.Attach(borrowing.ItemCopy);

            // Dont save the navigation properties if there is fk 

            _db.Entry(borrowing).Reference(b => b.User).CurrentValue = null;
            _db.Entry(borrowing).Reference(b => b.ItemCopy).CurrentValue = null;

            ct.ThrowIfCancellationRequested();
            await _db.Borrowings.AddAsync(borrowing, ct);
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateBorrowingAsync(Borrowing borrowing, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _db.Borrowings.Update(borrowing);
            await _db.SaveChangesAsync(ct);
        }
    }
}
