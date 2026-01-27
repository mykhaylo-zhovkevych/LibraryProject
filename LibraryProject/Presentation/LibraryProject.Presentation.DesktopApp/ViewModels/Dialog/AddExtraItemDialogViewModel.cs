using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.Dialog
{
    public partial class AddExtraItemDialogViewModel : DialogViewModel
    {
        [ObservableProperty] private string _title = "";
        [ObservableProperty] private string _message = "";
        [ObservableProperty] private string _confirmText = "";
        [ObservableProperty] private string _cancelText = "";

        [ObservableProperty] private int _countToAdd = 1;

        [ObservableProperty] private bool _confirmed;

        [RelayCommand]
        public void Confirm()
        {
            Confirmed = true;
            Close();
        }

        [RelayCommand]
        public void Cancel()
        {
            Confirmed = false;
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
