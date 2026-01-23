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
                //.AsNoTracking()
                .FirstOrDefaultAsync(i => i.Name == name && i.ItemType == itemType, ct);
        }

        public async Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            return await _db
                .Shelves
                //.AsNoTracking()
                .FirstOrDefaultAsync(s => s.ShelfId == id, ct);
        }

        public async Task<Shelf> GetOrCreateDefaultShelfAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Shelf? shelf = await _db
                .Shelves
                //.AsNoTracking()
                .FirstOrDefaultAsync(s => s.ShelfId == DefaultShelfId, ct);
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
                //.AsNoTracking()
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

        public async Task<ItemCopy?> GetCopyToBorrowAsync(Guid itemId, Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            // If user has a reservation than use copy
            ItemCopy? reservedForUser = await _db.ItemCopies
                //.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ItemId == itemId && !c.IsBorrowed && c.ReservedById == userId, ct);

            if (reservedForUser != null)
                return reservedForUser;

            // If not reserved, return first free copy
            return await _db.ItemCopies
                //.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ItemId == itemId && !c.IsBorrowed && c.ReservedById == null, ct);
        }

        
        public async Task<ItemCopy?> GetCopyToReserveAsync (Guid itemId, CancellationToken ct = default)
        {
            return await _db
                .ItemCopies
                //.AsNoTracking()
                .FirstOrDefaultAsync(c => c.ItemId == itemId && !c.IsBorrowed && c.ReservedById == null, ct);
        }

        public async Task UpdateCopyAsync(ItemCopy copy, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            //ItemCopy? tracked = _db.ItemCopies.Local.FirstOrDefault(i => i.Id == copy.Id);

            //if (tracked != null)
            //{
            //    // Update tracked instance values
            //    _db.Entry(tracked).CurrentValues.SetValues(copy);
            //}
            //else
            //{
            //    _db.ItemCopies.Attach(copy);
            //    _db.Entry(copy).State = EntityState.Modified;
            //}

            //_db.Entry(copy).Reference(c => c.Item).IsModified = false;
            //_db.Entry(copy).Reference(c => c.ReservedBy).IsModified = false;
            await _db.SaveChangesAsync(ct);
        }

        public async Task UpdateCirculationCountAsync(Guid itemId, int delta, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Item selectedItem = await _db.Items.FirstOrDefaultAsync(i => i.Id == itemId, ct) ?? throw new ArgumentException($"Item {itemId} not found");

            selectedItem.CirculationCount += delta;
            await _db.SaveChangesAsync(ct);
        }
    }
}
