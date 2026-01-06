using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories
{
    public class LibraryRepository : IShelfRepository
    {
        private readonly InMemoryStorage _storage;

        public LibraryRepository(InMemoryStorage storage)
        {
            _storage = storage;
        }

        // TODO: Add Validation
        public void RemoveItemFromStorage(Item item)
        {
            throw new NotImplementedException();
        }

        public void SaveItemToStorage(Item item)
        {
            _storage.Items.Add(item);
        }
    }
}
