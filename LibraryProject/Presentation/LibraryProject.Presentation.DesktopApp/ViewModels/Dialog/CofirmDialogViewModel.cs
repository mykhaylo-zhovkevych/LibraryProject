using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.Dialog
{
    public partial class ConfirmDialogViewModel : DialogViewModel
    {
        [ObservableProperty] private string _title = "A Dialog confirmation";
        [ObservableProperty] private string _message = "Confirm";
        [ObservableProperty] private string _confirmText = "OK";
        [ObservableProperty] private string _cancelText = "Cancel";



        [ObservableProperty]
        private bool _confirmed;

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

    }
}
