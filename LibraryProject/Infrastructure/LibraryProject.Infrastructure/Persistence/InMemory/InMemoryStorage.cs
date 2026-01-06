using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Persistence.InMemory
{
    public class InMemoryStorage
    {
        public List<Borrowing> Borrowings { get; private set; }
        public List<User> Users { get; private set; }
        public List<Shelf> Shelves { get; private set; }
        public List<Item> Items { get; private set; } = new List<Item>();


    }
}
