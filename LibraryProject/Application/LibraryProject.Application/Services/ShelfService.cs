using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services
{
    // This service uses the implementation from the infrastructure layer 
    public class ShelfService
    {
        private readonly IShelfRepository _shelfRepository;

        public ShelfService(IShelfRepository shelfRepository)
        {
            _shelfRepository = shelfRepository;
        }

        public void AddItemToShelf(Item item)
        {
            
            _shelfRepository.SaveItemToStorage(item);
        }

        public void RemoveItemFromShelf(Item item)
        {
            _shelfRepository.RemoveItemFromStorage(item);
        }

    }
}
