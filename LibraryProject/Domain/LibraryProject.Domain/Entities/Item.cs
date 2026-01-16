using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Author { get; set; }
        public int Year { get; }
        public string? Description { get; set; }
        public bool IsBorrowed { get; set; } = false;
        public string? CoverImagePath { get; private set; }

        // EF-Friendly Fk instead of only nvavigation 
        public Guid? ReservedById { get; internal set; }
        public User? ReservedBy { get; internal set; }
        public bool IsReserved => ReservedBy is not null;
        //EF-Friendly Fk to shefl (required for Shelves.Items persistence)
        public int ShelfId { get; set; }
        public ItemType ItemType { get; set; }
        // Must be updated after removing or updating new copies of the same item
        // Only the Application layer can interact with this property
        // derived data
        public int CirculationCount { get; set; }

        protected Item() { }
        public Item(string name, ItemType itemType, string author, int year, string? description = null, int circulationCount = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            ItemType = itemType;
            Author = author;
            Year = year;
            Description = description;
            CirculationCount = circulationCount;
        }

        public void SetCoverImagePath(string? relativePath)
        {
            CoverImagePath = string.IsNullOrWhiteSpace(relativePath) ? null : relativePath;
        }

        public bool CheckBorrowPossible()
        {
            if (IsBorrowed)
                return false;

            if (IsReserved)
                return false;

            return true;
        }

        public bool CheckReservePossible()
        {
            if (!IsBorrowed)
                return false;

            if (IsReserved)
                return false;

            return true;
        }

        public void BorrowItem()
        {
            IsBorrowed = true;
        }

        public void ReserveItem(User user)
        {
            ReservedBy = user;
            ReservedById = user.Id;
        }

        public void ReturnItem()
        {
            ReservedBy = null;
        }

        public void UpdateItemName(string newName)
        { 
            Name = newName;
        }

        public override string ToString()
        {
            return $"Item Id: {Id}, Item Name: {Name}, IsBorrowed: {IsBorrowed}" +
                $", IsReserved: {IsReserved}, ReservedBy (Id): {ReservedBy}";
        }
    }
}
