using LibraryProject.Presentation.DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Services
{
    public interface INavigationService
    {
        Task NavigateTo<T>() where T : ViewModelBase;
        void SetMainViewModel(MainViewModel mainViewModel);
    }
}
