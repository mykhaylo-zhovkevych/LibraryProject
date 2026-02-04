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

        public bool IsArchived { get; private set; }
        public bool ArchiveRequested { get; private set; }

        public bool CanArchivateNow() => !IsBorrowed && !IsReserved && ReservedById == null;

        protected ItemCopy() { }

        internal ItemCopy(Guid itemId)
        {
            ItemId = itemId;
        }

        public static ItemCopy CreateFor(Item item) => new ItemCopy(item.Id);


        public void ArchiveNow()
        {
            if (!CanArchivateNow()) 
            {
                throw new InvalidOperationException("Copy cannot be archived now (borrowed/reserved).");
            }
            IsArchived = true;
            ArchiveRequested = false;
        }

        public void RequestArchive()
        {
            ArchiveRequested = true;
            if (CanArchivateNow())
            {
                IsArchived = true;
            }
        }

        public bool CheckBorrowPossible(Guid userId)
        {
            if (IsArchived) 
                return false;
            if (IsBorrowed)
                return false;
            if (IsReserved && ReservedById != userId)
                return false;
            return true;
        }

        public bool CheckReservePossible()
        {
            if (IsArchived) 
                return false;
            if (IsBorrowed)
                return false;
            if (IsReserved)
                return false;
            return true;
        }

        public void BorrowItem()
        {
            if (IsArchived) throw new InvalidOperationException("Copy is archived.");
            if (IsBorrowed) throw new InvalidOperationException("Copy is already borrowed.");
            IsBorrowed = true;
        }

        public void ReserveById(Guid userId)
        {
            if (IsArchived) throw new InvalidOperationException("Copy is archived.");
            if (IsBorrowed) throw new InvalidOperationException("Copy is borrowed.");
            if (IsReserved) throw new InvalidOperationException("Copy is already reserved.");

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
            // auto-archive after return if it was requested
            if (ArchiveRequested && ReservedById == null)
            {
                IsArchived = true;
                ArchiveRequested = false;
            }
        }

        public void ReserveItem(User user)
        {
            if (IsArchived) throw new InvalidOperationException("Copy is archived.");
            if (IsBorrowed) throw new InvalidOperationException("Copy is borrowed.");
            if (IsReserved) throw new InvalidOperationException("Copy is already reserved.");

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
