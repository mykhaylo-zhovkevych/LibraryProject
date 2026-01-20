using LibraryProject.Domain.Entities;
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
        public DisplayedItem Item { get; set; }
        public DateTime LoanDate { get; set; }
        public DateTime DueDate { get; set; }
        public string Status { get; set; }


        public DisplayedBorrowing(DisplayedItem item, DateTime loanDate, DateTime dueDate, string status)
        {
            Item = item;
            LoanDate = loanDate;
            DueDate = dueDate;
            Status = status;
        }

    }
}
