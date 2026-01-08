using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
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


        public void RemoveItemFromShelf(Item item)
        {
            if (item == null)
            {
                throw new NonExistingItemException();
            }
            _authorizationService.EnsureAdmin();

            _itemRepository.RemoveFromShelf(item);
        }

        public Item CreateItem(string name, ItemType itemType)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Item name cannot be null or empty.");
            }
            _authorizationService.EnsureAdmin();

            Item newItem = new Item(name, itemType);
            AddItemToShelf(newItem);
            return newItem;
        }

        public bool CreateReservedItem(User user, Item item)
        {
            if (!item.CheckReservePossible())
            {
                throw new IsAlreadyReservedException(item);
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
                throw new NonExistingItemException();
            }
            if (string.IsNullOrWhiteSpace(newName))
            {
                throw new ArgumentException("Item name cannot be null or empty.");
            }
            _authorizationService.EnsureAdmin();

            item.UpdateItemName(newName);
            return true;   
        }

        public IEnumerable<Item> SearchForDesiredItem(
            string? nameContains = null,
            bool? isBorrowed = null,
            bool? isReserved = null,
            int? yearSelected = null,
            ItemType? itemType = null,
            Func<Item, bool>? customPredicate = null
            )
        {
            IEnumerable<Item> items = _itemRepository.GetAllItemsFromShelves().AsEnumerable();

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


        private void AddItemToShelf(Item item)
        {
            Item? interestedItem = _itemRepository.GetExistingItem(item.Name, item.ItemType);

            if (interestedItem != null && interestedItem.Id == item.Id)
            {
                throw new ItemAlreadyExistsWithThisIdException(item);
            }
            else
            {
                _itemRepository.AddToShelf(item);
            }
        }
    }
}
