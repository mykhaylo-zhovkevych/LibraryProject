using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibraryProject.Domain.Entities;

namespace LibraryProject.Application.Interfaces
{
    public interface IShelfRepository
    {
        void SaveItemToStorage(Item item);
        void RemoveItemFromStorage(Item item);

    }
}
