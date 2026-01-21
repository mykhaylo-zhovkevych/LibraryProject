using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Models
{
    public class DisplayedBorrowing
    {
        public Guid BorrowingId { get; }

        public Guid ItemCopyId { get; }
        public string BorrowingItemName { get; set; }
        public string BorrowingItemAuthror { get; set; }
        public string BorrowingItemType { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }


        public DisplayedBorrowing(Guid borrowingId, Guid itemCopyId, string borrowingItemName, string BorrowingItemAuthor, string borrowingItemType, DateTime loanDate, DateTime dueDate, string status)
        {
            BorrowingId = borrowingId;
            ItemCopyId = itemCopyId;
            BorrowingItemName = borrowingItemName;
            BorrowingItemAuthror = BorrowingItemAuthor;
            BorrowingItemType = borrowingItemType;
            LoanDate = loanDate;
            DueDate = dueDate;
            Status = status;
        }
    }
}
