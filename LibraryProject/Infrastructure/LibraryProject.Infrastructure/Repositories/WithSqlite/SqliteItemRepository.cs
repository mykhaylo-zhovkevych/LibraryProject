using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteItemRepository : IItemRepository
    {
        public Task AddToShelfAsync(Item item, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Item>> GetAllItemsFromShelvesAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Item?> GetExistingItemAsync(string name, ItemType itemType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Shelf> GetOrCreateDefaultShelfAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveFromShelfAsync(Item item, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
