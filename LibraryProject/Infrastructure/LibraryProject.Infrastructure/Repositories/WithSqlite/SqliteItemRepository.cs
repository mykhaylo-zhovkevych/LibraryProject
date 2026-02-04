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

        public SqliteItemRepository(LibraryDbContext db) => _db = db;

        public async Task<Item?> GetExistingItemAsync(string name, ItemType itemType, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Items
                .FirstOrDefaultAsync(i => i.Name == name && i.ItemType == itemType, ct);
        }

        public async Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db
                .Shelves
                .FirstOrDefaultAsync(s => s.ShelfId == id, ct);
        }

        public async Task<Shelf> GetOrCreateShelfAsync(int shelfId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            Shelf? shelf = await _db.Shelves.FirstOrDefaultAsync(s => s.ShelfId == shelfId, ct);

            if (shelf != null)
                return shelf;

            shelf = new Shelf(shelfId);
            _db.Shelves.Add(shelf);
            await _db.SaveChangesAsync(ct);

            return shelf;
        }

        public async Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _db.Items
                .Include(i => i.Copies)
                .ToListAsync(ct);
        }
        public async Task RemoveItemAsync(Item item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _db.Items.Remove(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task AddToShelfAsync(Item item, int shelfId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Shelf shelf = await GetOrCreateShelfAsync(shelfId, ct);

            item.SetShelf(shelf.ShelfId);
            _db.Items.Add(item);
            await _db.SaveChangesAsync(ct);
        }

        public async Task<ItemCopy?> GetCopyToBorrowAsync(Guid itemId, Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            // If user has a reservation than use copy
            ItemCopy? reservedForUser = await _db.ItemCopies
                .FirstOrDefaultAsync(c => c.ItemId == itemId && !c.IsArchived && !c.IsBorrowed && c.ReservedById == userId, ct);

            if (reservedForUser != null)
                return reservedForUser;

            // If not reserved, return first free copy
            return await _db.ItemCopies
                .FirstOrDefaultAsync(c => c.ItemId == itemId && !c.IsArchived && !c.IsBorrowed && c.ReservedById == null, ct);
        }

        
        public async Task<ItemCopy?> GetCopyToReserveAsync (Guid itemId, CancellationToken ct = default)
        {
            return await _db
                .ItemCopies
                .FirstOrDefaultAsync(c => c.ItemId == itemId && !c.IsArchived && !c.IsBorrowed && c.ReservedById == null, ct);
        }

        public async Task UpdateCopyAsync(ItemCopy copy, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateCirculationCountAsync(Guid itemId, int delta, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Item selectedItem = await _db.Items.FirstOrDefaultAsync(i => i.Id == itemId, ct) ?? throw new ArgumentException($"Item {itemId} not found");

            selectedItem.CirculationCount += delta;
            await _db.SaveChangesAsync(ct);
        }

        public async Task<Item?> GetItemByIdAsync(Guid itemId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();

            return await _db.Items
                .Include(i => i.Copies)
                .FirstOrDefaultAsync(i => i.Id == itemId, ct);
        }

        public async Task UpdateItemAsync(Item item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            await _db.SaveChangesAsync(ct);
        }

        public async Task InsertCopiesToItemAsync(Guid itemId, int count, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            // Item tracked 
            Item item = await _db.Items.FirstOrDefaultAsync(i => i.Id == itemId, ct) ?? throw new ArgumentException($"Item {itemId} not found");
            List<ItemCopy> newCopies = Enumerable.Range(0, count).Select(_ => ItemCopy.CreateFor(item)).ToList();

            await _db.ItemCopies.AddRangeAsync(newCopies, ct);
            item.CirculationCount += count;

            await _db.SaveChangesAsync(ct);
        }

        public Task<bool> HasAnyReservationsAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return _db.ItemCopies.AnyAsync(c => c.ReservedById == userId, ct);
        }

        public async Task<List<ItemCopy>> GetReservedCopiesByUserAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.ItemCopies.Where(c => c.ReservedById == userId).Include(c => c.Item).ToListAsync(ct);
        }
    }
}
