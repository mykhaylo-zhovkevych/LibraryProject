using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteItemRepository : IItemRepository
    {
        private readonly LibraryDbContext _db;
        private const int DefaultShelfId = 100;

        public SqliteItemRepository(LibraryDbContext db) => _db = db;

        public async Task<Item?> GetExistingItemAsync(string name, ItemType itemType, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Items
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Name == name && i.ItemType == itemType, ct);
        }

        public async Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Shelves.FirstOrDefaultAsync(s => s.ShelfId == id, ct);
        }

        public async Task<Shelf> GetOrCreateDefaultShelfAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Shelf? shelf = await _db.Shelves.FirstOrDefaultAsync(s => s.ShelfId == DefaultShelfId, ct);
            if (shelf != null)
            {
                return shelf;
            }

            shelf = new Shelf(DefaultShelfId);
            await _db.Shelves.AddAsync(shelf, ct);
            await _db.SaveChangesAsync(ct);
            return shelf;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _db.Items
                .AsNoTracking()
                .Include(i => i.Copies)
                .ToListAsync(ct);
        }
        public async Task RemoveItemAsync(Item item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _db.Items.Remove(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task AddToShelfAsync(Item item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Shelf shelf = await GetOrCreateDefaultShelfAsync(ct);
            shelf.AddItem(item);

            await _db.SaveChangesAsync(ct);
        }

        public async Task<ItemCopy?> GetFirstFreeCopyAsync(Guid itemId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _db.ItemCopies
                .Include(c => c.Item)
                .FirstOrDefaultAsync(c => c.ItemId == itemId && !c.IsBorrowed && c.ReservedById == null, ct);
        }

        public async Task UpdateCopyAsync(ItemCopy copy, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            // TODO: issue with the reservationa and borrowing return again reservation , because the item attaches aggain
            _db.Entry(copy).Reference(c => c.Item).CurrentValue = null;
            _db.Entry(copy).Reference(c => c.ReservedBy).CurrentValue = null;

            _db.ItemCopies.Attach(copy);
            _db.Entry(copy).State = EntityState.Modified;

            await _db.SaveChangesAsync(ct);
        }
    }
}
