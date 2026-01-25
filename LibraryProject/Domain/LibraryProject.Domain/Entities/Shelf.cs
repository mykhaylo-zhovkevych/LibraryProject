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
        public IReadOnlyCollection<Item> Items => _items;

        protected Shelf() { }

        public Shelf(int? shelfId = null)
        {
            if (shelfId.HasValue)
            {
                ShelfId = shelfId.Value;
            }
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
            item.SetShelf(ShelfId);
        }

        public bool RemoveItem(Item item)
        {
            return _items.Remove(item);
        }
    }
}
