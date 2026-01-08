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
        public int Year { get; }
        public bool IsBorrowed { get; set; } = false;
        public User? ReservedBy { get; internal set; }
        public bool IsReserved => ReservedBy is not null;
        public ItemType ItemType { get; set; }

        public Item(string name, ItemType itemType)
        {
            Id = Guid.NewGuid();
            Name = name;
            ItemType = itemType;
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
