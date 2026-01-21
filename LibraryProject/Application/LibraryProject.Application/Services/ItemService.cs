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

        public ItemService(IItemRepository itemfRepository, IAuthorizationService authorizationService)
        {
            _itemRepository = itemfRepository;
            _authorizationService = authorizationService;
        }

        public async Task RemoveItemByIdAsync(Item item, CancellationToken ct)
        {
            if (item == null)
            {
                throw new NonexistentItemException();
            }
            _authorizationService.EnsureAdmin();

            // Get the item by id
            var existingItem = await _itemRepository.GetExistingItemAsync(item.Name, item.ItemType, ct);

            if (existingItem == null || existingItem.Id != item.Id)
            {
                throw new NonexistentItemException();
            }
            else
            {
                await _itemRepository.RemoveFromShelfAsync(existingItem, ct);
                existingItem.CirculationCount--;
            }
        }


        public async Task RemoveAllItemsAsync(Item item, CancellationToken ct)
        {
            if (item == null)
            {
                throw new NonexistentItemException();
            }
            _authorizationService.EnsureAdmin();

            var itemsToRemove = await _itemRepository.GetAllItemsFromShelvesAsync(ct);
            var itemsMatching = itemsToRemove.Where(i => i.Name == item.Name && i.ItemType == item.ItemType && i.Author == item.Author && i.Year == item.Year);
            foreach (var it in itemsMatching)
            {
                await _itemRepository.RemoveFromShelfAsync(it, ct);
            }
        }

        public async Task CreateItemWithAmount(string name, ItemType itemType, string author, int year, string? description, int circulationCount, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Item name cannot be null or empty.");
            }

            if (circulationCount <= 0)
            {
                throw new ArgumentException("circulationCount must be > 0.");
            }

            // _authorizationService.EnsureAdmin();

            Item item = new Item(name, itemType, author, year, description, circulationCount);

            for (int i = 1; i <= circulationCount; i++)
            {
                item.Copies.Add(new ItemCopy { });
            }

            await AddItemToShelf(item, ct);
        }

        public async Task<bool> CreateReservedItem(User user, Item item, CancellationToken ct)
        {
            //if (!item.CheckReservePossible())
            //{
            //    throw new ItemUsedByException(item);
            //}

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (item == null) throw new NonexistentItemException();

            ItemCopy copyToReserve = item.Copies.FirstOrDefault(c => c.IsBorrowed && c.ReservedById == null);
            if (copyToReserve == null)
            {
                throw new ArgumentException($"No copy can be reserved for {item.Name}.");
            }

            copyToReserve.ReserveItem(user);
            await _itemRepository.UpdateCopyAsync(copyToReserve, ct);
            return true;

        }

        public async Task<bool> CancelReservation(User user, Item item)
        {
            //if (item.ReservedBy != user)
            //{
            //    throw new ArgumentException($"{user.Name} has no reservation for {item.Name}");
            //}

            if (user == null) throw new ArgumentNullException(nameof(user));
            if (item == null) throw new NonexistentItemException();

            _authorizationService.EnsureAuthenticated();

            var reservedCopy = item.Copies.FirstOrDefault(c => c.ReservedById == user.Id);
            if (reservedCopy == null)
            {
                throw new ArgumentException($"{user.Name} has no reservation for {item.Name}");
            }

            reservedCopy.ReturnItem();
            await _itemRepository.UpdateCopyAsync(reservedCopy);

            return true;
        }

        public bool ChangeItemName(Item item, string newName)
        {
            if (item == null)
            {
                throw new NonexistentItemException();
            }
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Item name cannot be null or empty.");
            }
            _authorizationService.EnsureAdmin();

            item.UpdateItemName(newName);
            return true;   
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
            IEnumerable<Item> items = await _itemRepository.GetAllItemsFromShelvesAsync();

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

            if (yearSelected.HasValue)
            {
                items = items.Where(i => i.Year == yearSelected.Value);
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


        private async Task<Item> AddItemToShelf(Item item, CancellationToken ct)
        {
            Item? interestedItem = await _itemRepository.GetExistingItemAsync(item.Name, item.ItemType);

            if (interestedItem != null && interestedItem.Id == item.Id)
            {
                throw new ItemAlreadyExistsWithThisIdException(item);
            }
            else
            {
                await _itemRepository.AddToShelfAsync(item);
                return item;
            }
        }
    }
}
