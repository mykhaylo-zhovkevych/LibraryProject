using CommunityToolkit.Mvvm.ComponentModel;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.ViewModels.Dialog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class PageViewModel : ViewModelBase
    {
        [ObservableProperty]
        private ApplicationPageNames _pageName;

        [ObservableProperty]
        private DialogViewModel? _currentDialog;
    }
}
