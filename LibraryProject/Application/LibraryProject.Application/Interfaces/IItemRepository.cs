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
        void SaveItemToStorage(Item item);
        void RemoveItemFromStorage(Item item);
        Item? GetExistingItem(string name, ItemType itemType);
        Shelf? GetShelfById(int id);


    }
}
