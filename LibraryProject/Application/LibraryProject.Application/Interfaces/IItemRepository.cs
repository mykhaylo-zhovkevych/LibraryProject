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
        Task AddToShelfAsync(Item item,int shelfId, CancellationToken ct = default);
        Task RemoveItemAsync(Item item, CancellationToken ct = default);
        Task UpdateCopyAsync(ItemCopy copy, CancellationToken ct = default);
        Task UpdateItemAsync(Item item, CancellationToken ct = default);
        Task InsertCopiesToItemAsync(Guid itemId, int count, CancellationToken ct = default);

        Task<Item?> GetItemByIdAsync(Guid itemId, CancellationToken ct = default);

        Task<Shelf> GetOrCreateDefaultShelfAsync(int shelfId, CancellationToken ct = default);
        Task<Item?> GetExistingItemAsync(string name, ItemType itemType, CancellationToken ct = default);

        Task<Shelf?> GetShelfByIdAsync(int id, CancellationToken ct = default);
        Task<IEnumerable<Item>> GetAllItemsAsync(CancellationToken ct = default);

        Task<ItemCopy?> GetCopyToBorrowAsync(Guid itemId, Guid userId, CancellationToken ct = default);
        Task<ItemCopy?> GetCopyToReserveAsync(Guid itemId, CancellationToken ct);

        Task UpdateCirculationCountAsync(Guid itemId, int delta, CancellationToken ct = default);

    }
}