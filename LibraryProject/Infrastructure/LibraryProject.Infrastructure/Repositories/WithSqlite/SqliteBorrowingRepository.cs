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

        public async Task<Borrowing?> GetActiveBorrowingAsync(Guid userId, Guid itemId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Borrowings
                .FirstOrDefaultAsync(b => b.UserId == userId && b.ItemId == itemId && !b.IsReturned, ct);
        }

        public async Task<List<Borrowing>> GetActiveBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Borrowings
                .AsNoTracking()
                .Include(b => b.Item) // Was added cause of sql
                .Where(b => b.UserId == userId && b.IsReturned == null)
                .ToListAsync(ct);
        }

        public async Task<List<Borrowing>> GetInactiveBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Borrowings
                .AsNoTracking()
                .Include(b => b.Item)
                .Where(b => b.UserId == userId && b.IsReturned != null)
                .ToListAsync(ct);
        }

        public async Task<List<Borrowing>> GetAllBorrowingsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Borrowings
                .AsNoTracking()
                .Include(b => b.Item)
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
            //_db.Attach(borrowing.Item);

            _db.Entry(borrowing.User).State = EntityState.Unchanged;
            _db.Entry(borrowing.Item).State = EntityState.Unchanged;

            ct.ThrowIfCancellationRequested();
            await _db.Borrowings.AddAsync(borrowing, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
