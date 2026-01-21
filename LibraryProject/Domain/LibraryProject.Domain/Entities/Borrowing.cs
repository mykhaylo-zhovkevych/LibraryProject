using LibraryProject.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Borrowing
    {
        public Guid BorrowingId { get; private set; }

        public Guid UserId { get; private set; }
        public User User { get; init; }

        public Guid ItemCopyId { get; set; }
        public ItemCopy ItemCopy { get; set; }

        public Policy Policy { get; }
        public DateTime LoanDate { get; private set; }
        public DateTime DueDate { get; private set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned => ReturnDate.HasValue;

        public uint RemainingExtensionCredits { get; private set; }

        protected Borrowing() { }
        public Borrowing(User user, ItemCopy copy, Policy policy)
        {
            User = user;
            UserId = user.Id;
            ItemCopy = copy;
            ItemCopyId = copy.Id;
            Policy = policy;
            LoanDate = DateTime.Today;
            DueDate = LoanDate.AddDays(policy.LoanPeriodInDays);

            RemainingExtensionCredits = policy.Extensions;
        }

        public bool Extend()
        {
            if (ItemCopy.IsReserved)
            {
                // throw new ItemUsedByException();
            }

            if (IsReturned) return false;

            if (RemainingExtensionCredits == 0)
            {
                return false;
            }

            DueDate = DueDate.AddDays(Policy.LoanPeriodInDays);
            RemainingExtensionCredits--;

            return true;
        }

        public void ReturnBorrowing(Borrowing borrowing)
        {
            borrowing.ReturnDate = DateTime.Now;
            borrowing.ItemCopy.IsBorrowed = false;

        }
    }
}
