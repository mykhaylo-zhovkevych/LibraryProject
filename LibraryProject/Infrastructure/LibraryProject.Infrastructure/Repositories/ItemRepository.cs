using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly InMemoryStorage _storage;

        public ItemRepository(InMemoryStorage storage)
        {
            _storage = storage;
        }

        public Item? GetExistingItem(string name, ItemType itemType)
        {
            return _storage.Shelves
                .SelectMany(s => s.Items)
                .FirstOrDefault(i => i.Name == name && i.ItemType == itemType);
        }

        public Shelf? GetShelfById(int id)
        {
            return _storage.Shelves
                .FirstOrDefault(s => s.ShelfId == id);
        }

        // TODO: Add Validation not business rule, but data integrity
        public void RemoveItemFromStorage(Item item)
        {
            _storage.Items.Remove(item);
        }

        public void SaveItemToStorage(Item item)
        {
            _storage.Items.Add(item);
        }

        public void AddShelf(Shelf shelf) => _storage.Shelves.Add(shelf);
    }
}
