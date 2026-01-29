using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
using LibraryProject.Domain.Exceptions.Nonexistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAuthorizationService _authorizationService;
        private const int DefaultShelfId = 101;

        public ItemService(IItemRepository itemfRepository, IAuthorizationService authorizationService)
        {
            _itemRepository = itemfRepository;
            _authorizationService = authorizationService;
        }

        public async Task CreateItemWithAmount(string name, ItemType itemType, string author, int year, string? description, int circulationCount, int? shelfId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Item name cannot be null or empty.");
            }

            if (circulationCount <= 0)
            {
                throw new ArgumentException("circulationCount must be less than 0.");
            }

            _authorizationService.EnsureAdmin();

            Item item = new Item(name.Trim(),itemType, author.Trim(), year, description: string.IsNullOrWhiteSpace(description) ? null : description.Trim(), 0);

            item.AddCopies(circulationCount);
            item.CirculationCount = circulationCount;

            await AddItemToShelf(item, shelfId, ct);
        }

        public async Task CreateReservedItemAsync(User user, Item item, CancellationToken ct)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (item == null) throw new NonexistentItemException();

            ItemCopy? copyToReserve = await _itemRepository.GetCopyToReserveAsync(item.Id, ct);
            if (copyToReserve == null)
            {
                throw new ArgumentException($"No copy can be reserved for {item.Name}.");
            }

            if (!copyToReserve.CheckReservePossible())
            {
                throw new ItemUsedByException(item);
            }

            copyToReserve.ReserveItem(user);
            await _itemRepository.UpdateCopyAsync(copyToReserve, ct);
        }


        public async Task RemoveItemAsync(Item item, CancellationToken ct)
        {
            if (item == null)
            {
                throw new NonexistentItemException();
            }
            _authorizationService.EnsureAdmin();

            IEnumerable<Item> itemsToRemove = await _itemRepository.GetAllItemsAsync(ct);
            IEnumerable<Item> itemsMatching = itemsToRemove.Where(i => i.Name == item.Name &&
                                                                  i.ItemType == item.ItemType &&
                                                                  i.Author == item.Author &&
                                                                  i.Year == item.Year);

            Item foundItem = itemsMatching.FirstOrDefault() ?? throw new NonexistentItemException();

            await _itemRepository.RemoveItemAsync(foundItem, ct);
        }


        public async Task AddCopiesToItemAsync(Guid itemId, int count, CancellationToken ct)
        {
            if (count <= 0) throw new ArgumentException("Invalid copies value.");
            _authorizationService.EnsureAdmin();

            await _itemRepository.InsertCopiesToItemAsync(itemId, count, ct);
        }


        public async Task RemoveItemCopiesByIdAsync(Item item, int count, CancellationToken ct)
        {
            if (item == null)
            {
                throw new NonexistentItemException();
            }
            _authorizationService.EnsureAdmin();

            IEnumerable<Item> items = await _itemRepository.GetAllItemsAsync(ct);

            Item foundItem = items.FirstOrDefault(i => i.Name == item.Name && i.ItemType == item.ItemType && i.Author == item.Author && i.Year == item.Year) ?? throw new NonexistentItemException();

            if (count <= 0) throw new ArgumentException("Invalid copies value.");

            List<ItemCopy> toRemoveItem = foundItem.Copies.Where(c => !c.IsBorrowed && c.ReservedById == null).Take(count).ToList();

            if (toRemoveItem.Count < count)
            {
                throw new ArgumentException("Not enought copies to remove.");
            }

            foreach (ItemCopy currentItemCopy in toRemoveItem) 
            { 
                foundItem.Copies.Remove(currentItemCopy);
            }

            foundItem.CirculationCount -= count;
            await _itemRepository.UpdateItemAsync(foundItem, ct);
        }


        public async Task CancelReservation(User user, Item item)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (item == null) throw new NonexistentItemException();

            _authorizationService.EnsureAuthenticated();

            ItemCopy? reservedCopy = item.Copies.FirstOrDefault(c => c.ReservedById == user.Id);
            if (reservedCopy == null)
            {
                throw new ArgumentException($"{user.Name} has no reservation for {item.Name}");
            }

            reservedCopy.ReturnItem();
            await _itemRepository.UpdateCopyAsync(reservedCopy);

        }

        public async Task UpdateItemAsync(Guid itemId, string title, string author, int year, string? description, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Titel is empty.");
            }
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Autor is empty.");
            }
            if (year <= 0)
            {
                throw new ArgumentException("Year invalid.");
            }

            _authorizationService.EnsureAdmin();
            Item item = await _itemRepository.GetItemByIdAsync(itemId, ct) ?? throw new NonexistentItemException();

            item.UpdateItemName(title.Trim());
            item.UpdateAuthor(author.Trim());
            item.UpdateYear(year);
            item.UpdateDescription(string.IsNullOrWhiteSpace(description) ? null : description.Trim());

            await _itemRepository.UpdateItemAsync(item, ct);
        }

        public async Task<IEnumerable<Item>> SearchForDesiredItem(
            string? nameContains = null,
            bool? isBorrowed = null,
            bool? isReserved = null,
            int? yearSelected = null,
            ItemType? itemType = null,
            Func<Item, bool>? customPredicate = null
            )
        {
            IEnumerable<Item> items = await _itemRepository.GetAllItemsAsync();

            string term = nameContains?.Trim();

            if (!string.IsNullOrWhiteSpace(term))
            {
                items = items.Where(i => i.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            if (isBorrowed.HasValue)
            {
                if (isBorrowed.Value)
                {
                    items = items.Where(i => i.Copies.Any(c => c.IsBorrowed));
                }
                else
                {
                    items = items.Where(i => i.Copies.Any(c => !c.IsBorrowed));
                }
                    
            }

            if (isReserved.HasValue)
            {
                if (isReserved.Value)
                {
                    items = items.Where(i => i.Copies.Any(c => c.ReservedById != null));
                }
                else
                {
                    items = items.Where(i => i.Copies.Any(c => c.ReservedById == null));
                }
            }

            if (yearSelected.HasValue)
            {
                items = items.Where(i => i.Year == yearSelected.Value);
            }

            if (itemType != null)
            {
                items = items.Where(i => itemType.Equals(i.ItemType));
            }

            if (customPredicate != null)
            {
                items = items.Where(customPredicate);
            }
            return items.ToList();
        }

        private async Task<Item> AddItemToShelf(Item item, int? shelfId, CancellationToken ct)
        {
            Item? existingItem = await _itemRepository.GetExistingItemAsync(item.Name, item.ItemType, ct);
            if (existingItem != null)
            {
                throw new ItemAlreadyExistsWithThisIdException(item);
            }

            int targetShelfId = shelfId ?? DefaultShelfId;

            await _itemRepository.AddToShelfAsync(item, targetShelfId, ct);
            return item;
        }
    }
}
