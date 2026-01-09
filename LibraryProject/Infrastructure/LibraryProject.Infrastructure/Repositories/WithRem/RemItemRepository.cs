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

        public RemItemRepository(LibraryStorage storage)
        {
            _storage = storage;
        }

        public Task<Item?> GetExistingItemAsync(string name, ItemType itemType, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Item? query = _storage.Shelves
                .SelectMany(s => s.Items)
                .FirstOrDefault(i => i.Name == name && i.ItemType == itemType);
            return Task.FromResult(query);
        }

        public Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Shelf? query = _storage.Shelves
                .FirstOrDefault(s => s.ShelfId == id);
            return Task.FromResult(query);
        }


        public Task<Shelf> GetOrCreateDefaultShelfAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Shelf? shelf = _storage.Shelves.FirstOrDefault(s => s.ShelfId == DefaultShelfId);
            if (shelf == null)
            {
                shelf = new Shelf(DefaultShelfId);
                AddShelf(shelf);
            }
            return Task.FromResult(shelf);
        }

        public Task<IEnumerable<Item>> GetAllItemsFromShelvesAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            IEnumerable<Item> query = _storage.Shelves.SelectMany(s => s.Items);
            return Task.FromResult(query);
        }

        public async Task AddToShelfAsync(Item item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            Shelf shelf = await GetOrCreateDefaultShelfAsync(ct);
            shelf.AddItem(item);
        }

        public Task RemoveFromShelfAsync(Item item, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            foreach (var shelf in _storage.Shelves)
            {
                shelf.RemoveItem(item);
            }
            return Task.CompletedTask;
        }

        private void AddShelf(Shelf shelf) => _storage.Shelves.Add(shelf);
    }
}
