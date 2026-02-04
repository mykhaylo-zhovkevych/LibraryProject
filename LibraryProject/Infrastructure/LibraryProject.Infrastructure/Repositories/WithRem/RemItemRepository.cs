using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithRem
{
    public class RemItemRepository : IItemRepository
    {
        private readonly LibraryStorage _storage;
        private const int DefaultShelfId = 100;

        public RemItemRepository(LibraryStorage storage) => _storage = storage;

        // private void AddShelf(Shelf shelf) => _storage.Shelves.Add(shelf);

        public Task UpdateCopyAsync(ItemCopy copy, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCirculationCountAsync(Guid itemId, int delta, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ItemCopy?> GetCopyToBorrowAsync(Guid itemId, Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<ItemCopy?> GetCopyToReserveAsync(Guid itemId, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<Item?> GetItemByIdAsync(Guid itemId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateItemAsync(Item item, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task InsertCopiesToItemAsync(Guid itemId, int count, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task AddToShelfAsync(Item item, int shelfId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveItemAsync(Item item, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Shelf> GetOrCreateShelfAsync(int shelfId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Item?> GetExistingItemAsync(string name, ItemType itemType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasAnyReservationsAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<ItemCopy>> GetReservedCopiesByUserAsync(Guid userId, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
