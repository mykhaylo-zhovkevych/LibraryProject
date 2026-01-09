using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Persistence.InMemory
{
    public class LibraryStorage
    {
        public List<Account> Accounts { get; } = new();
        public Dictionary<(UserType UserType, ItemType ItemType), Policy> Policies { get; private set; } = new();
        public List<Borrowing> Borrowings { get; } = new();
        public List<User> Users { get; } = new();
        public List<Shelf> Shelves { get; } = new();

    }
}
