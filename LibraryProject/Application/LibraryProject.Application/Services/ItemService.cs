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

        public async Task RemoveItemAsync(Item item, CancellationToken ct)
        {
            if (item == null)
            {
                throw new NonexistentItemException();
            }
            _authorizationService.EnsureAdmin();

            await _itemRepository.RemoveFromShelfAsync(item, ct);
        }

        public async Task<Item> CreateItem(string name, ItemType itemType, string author, int year, string? description, int circulationCount, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Item name cannot be null or empty.");
            }
            _authorizationService.EnsureAdmin();

            Item newItem = new Item(name, itemType, author, year, description, circulationCount);
            await AddItemToShelf(newItem, ct);
            return newItem;
        }

        public bool CreateReservedItem(User user, Item item)
        {
            if (!item.CheckReservePossible())
            {
                throw new ItemUsedByException(item);
            }
            _authorizationService.EnsureAuthenticated();
            item.ReserveItem(user);
            return true;
        }

        public bool CancelReservation(User user, Item item)
        {
            if (item.ReservedBy != user)
            {
                throw new ArgumentException($"{user.Name} has no reservation for {item.Name}");
            }
            _authorizationService.EnsureAuthenticated();
            item.ReturnItem();
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

            if (isBorrowed != null)
            {
                items = items.Where(i => i.IsBorrowed == isBorrowed);
            }

            if (yearSelected.HasValue)
            {
                items = items.Where(i => i.Year == yearSelected.Value);
            }

            if (isReserved.HasValue)
            {
                items = items.Where(i => i.IsReserved == isReserved);
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
