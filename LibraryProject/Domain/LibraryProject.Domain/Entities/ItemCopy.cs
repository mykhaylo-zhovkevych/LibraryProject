using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class ItemCopy
    {
        public Guid Id { get; init; } = Guid.NewGuid();

        public Guid ItemId { get; private set; }
        public Item Item { get; private set; } = null!;

        public bool IsBorrowed { get; internal set;}
        public bool IsReserved => ReservedById != null;
        public Guid? ReservedById { get; private set; }
        public User? ReservedBy { get; private set; }

        public bool CheckBorrowPossible(Guid userId)
        {
            if (IsBorrowed)
                return false;
            if (IsReserved && ReservedById != userId)
                return false;
            return true;

        }

        protected ItemCopy() { }

        internal ItemCopy(Guid itemId)
        {
            ItemId = itemId;
        }

        public static ItemCopy CreateFor(Item item) => new ItemCopy(item.Id);

        public bool CheckReservePossible()
        {
            if (IsBorrowed)
                return false;

            if (IsReserved)
                return false;

            return true;
        }

        public void BorrowItem()
        {
            IsBorrowed = true;
        }

        public void ReserveById(Guid userId)
        {
            ReservedById = userId;
            ReservedBy = null;
        }

        public void CancelReservation()
        {
            ReservedBy = null;
            ReservedById = null;
        }

        public void ReturnFromBorrowing()
        {
            IsBorrowed = false;
        }

        public void ReserveItem(User user)
        {
            ReservedBy = user;
            ReservedById = user.Id;
        }

        public void ReturnItem()
        {
            ReservedBy = null;
            ReservedById = null;
        }
    }
}
