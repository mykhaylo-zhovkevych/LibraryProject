using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Persistence.InMemory
{
    // TODO: Separate class
    public class InMemoryStorage
    {
        public Dictionary<(UserType UserType, ItemType ItemType), Policy> Policies { get; private set; }
        public List<Borrowing> Borrowings { get; private set; }
        public List<User> Users { get; private set; }
        public List<Shelf> Shelves { get; private set; }
        public List<Item> Items { get; private set; }


    }
}
