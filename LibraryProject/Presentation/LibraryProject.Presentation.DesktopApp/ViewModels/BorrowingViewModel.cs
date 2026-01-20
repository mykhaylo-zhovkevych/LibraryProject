using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class BorrowingViewModel : PageViewModel
    {
        public ObservableCollection<DisplayedBorrowing> Borrowings { get; set; } = new();
        public BorrowingViewModel()
        {
            PageName = ApplicationPageNames.Borrowing;
        }




    }
}
