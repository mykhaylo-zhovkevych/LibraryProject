using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.Dialog
{
    public partial class BorrowingHistoryDialogViewModel : DialogViewModel
    {
     
        [ObservableProperty] private string _title = "";
        [ObservableProperty] private string _message = "";
        [ObservableProperty] private string _confirmText = "";
        [ObservableProperty] private string _cancelText = "";

        public ObservableCollection<Borrowing> Borrowings { get; } = new();

        [ObservableProperty]
        private bool _confirmed;

        public BorrowingHistoryDialogViewModel(IEnumerable<Borrowing> borrowings)
        {
            foreach (var b in borrowings)
            {
                Borrowings.Add(b);
            }
        }

        [RelayCommand]
        private void Confirm()
        {
            Confirmed = true;
            Close();
        }

        public async Task<bool> WaitConfirmationAsync()
        {
            if (!Confirmed)
                await WaitDialogAsnyc();

            return Confirmed;
        }
    }
}
