using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interfaces
{
    public interface IItemRepository
    {
        Task AddToShelfAsync(Item item, CancellationToken ct = default);
        Task RemoveFromShelfAsync(Item item, CancellationToken ct = default);
        Task UpdateCopyAsync(ItemCopy copy, CancellationToken ct = default);

        Task<Shelf> GetOrCreateDefaultShelfAsync(CancellationToken ct = default);
        Task<Item?> GetExistingItemAsync(string name, ItemType itemType, CancellationToken ct = default);
        Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Item>> GetAllItemsFromShelvesAsync(CancellationToken ct = default);

        Task<ItemCopy?> GetFirstFreeCopyAsync(Guid itemId, CancellationToken ct = default);


    }
}
