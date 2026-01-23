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

        public async Task CreateItemWithAmount(string name, ItemType itemType, string author, int year, string? description, int circulationCount, CancellationToken ct)
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

            Item item = new Item(name, itemType, author, year, description, circulationCount);

            for (int i = 1; i <= circulationCount; i++)
            {
                item.Copies.Add(new ItemCopy {});
            }

            await AddItemToShelf(item, ct);
        }

        public async Task CreateReservedItemAsync(User user, Item item, CancellationToken ct)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (item == null) throw new NonexistentItemException();

            // Better not to use untracked object graph and causes issues with attaching 
            //ItemCopy? copyToReserve = item.Copies.FirstOrDefault(c => !c.IsBorrowed && c.ReservedById == null);
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
            
            // await _itemRepository.UpdateCirculationCountAsync(foundItem.Id, -1, ct);
            await _itemRepository.RemoveItemAsync(foundItem, ct);
            // foundItem.CirculationCount--;
        }

        public async Task RemoveItemCopiesByIdAsync(Item item, int count, CancellationToken ct)
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

            Item? foundItem = itemsMatching.FirstOrDefault() ?? throw new NonexistentItemException();

            if (count <= 0 || count > foundItem.Copies.Count)
            {
                throw new ArgumentException("Invalid copies value.");
            }
            else if (count == foundItem.Copies.Count)
            {
                foreach (var i in foundItem.Copies)
                {
                    foundItem.Copies.Remove(i);
                }
            }
            else             
            {
                for (int i = 0; i < count; i++)
                {
                    foundItem.Copies.Remove(foundItem.Copies.Last());
                }
            }
        }

        public async Task CancelReservation(User user, Item item)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (item == null) throw new NonexistentItemException();

            _authorizationService.EnsureAuthenticated();

            ItemCopy reservedCopy = item.Copies.FirstOrDefault(c => c.ReservedById == user.Id);
            if (reservedCopy == null)
            {
                throw new ArgumentException($"{user.Name} has no reservation for {item.Name}");
            }

            reservedCopy.ReturnItem();
            await _itemRepository.UpdateCopyAsync(reservedCopy);

        }

        public void ChangeItemName(Item item, string newName)
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
