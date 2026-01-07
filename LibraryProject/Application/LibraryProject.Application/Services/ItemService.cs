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

        public ItemService(IItemRepository itemfRepository)
        {
            _itemRepository = itemfRepository;
        }

        // TODO: Add Validation business rule
        public void AddItemToShelf(Item item)
        {
            _itemRepository.AddToShelf(item);
        }


        //public void RemoveItemFromShelf(Item item)
        //{
        //    _itemRepository.RemoveFromShelf(item);
        //}

        public (bool Success, Item? Item) CreateNewItem(string name, ItemType itemType)
        {

   
            if (string.IsNullOrEmpty(name))
            {
                return (false, null);
            }
            Item newItem = new Item(name, itemType);

            AddItemToShelf(newItem);

            return (true, newItem);
        }

        // TODO: make the rest of ex like this one
        public bool CreateReservedItem(User user, Item item)
        {
            if (!item.CheckReservePossible())
            {
                throw new IsAlreadyReservedException(item);
            }

            item.ReserveItem(user);
            return true;
        }

        // TODO: add the UpdateUserProfile, DleteUserProfile
        public bool CancelReservation(User user, Item item)
        {
            if (item.ReservedBy != user)
            {
                throw new ArgumentException($"{user.Name} has no reservation for {item.Name}");
            }

            item.ReturnItem();
            return true;
        }



    }
}
