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
        private const int DefaultShelfId = 100;

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


        public Shelf GetOrCreateDefaultShelf()
        {
            var shelf = _storage.Shelves.FirstOrDefault(s => s.ShelfId == DefaultShelfId);
            if (shelf == null)
            {
                shelf = new Shelf(DefaultShelfId);
                AddShelf(shelf);
            }
            return shelf;
        }

        public List<Item> GetAllItemsFromShelves()
        {
            return _storage.Shelves.SelectMany(s => s.Items).ToList();
        }

        public void AddToShelf(Item item)
        {
            var shelf = GetOrCreateDefaultShelf();
            shelf.AddItem(item);
        }

        public void RemoveFromShelf(Item item)
        {
            foreach (var shelf in _storage.Shelves)
            {
                shelf.RemoveItem(item);
            }
        }
        private void AddShelf(Shelf shelf) => _storage.Shelves.Add(shelf);

    }
}
