using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Shelf
    {
        public int ShelfId { get; private set; }

        private readonly List<Item> _items = new();
        public IReadOnlyList<Item> Items => _items;

        public Shelf(int? shelfId == null)
        {
            ShelfId = shelfId ?? new Random().Next(1, int.MaxValue);
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public bool RemoveItem(Item item)
        {
            return _items.Remove(item);
        }
    }
}
