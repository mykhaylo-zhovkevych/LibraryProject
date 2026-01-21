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

        public Guid ItemId { get; set; }
        public Item Item { get; set; } = null!;

        public bool IsBorrowed { get; set; }
        public Guid? ReservedById { get; set; }
        public User? ReservedBy { get; set; }

        public bool IsReserved => ReservedById != null;

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


    }
}
