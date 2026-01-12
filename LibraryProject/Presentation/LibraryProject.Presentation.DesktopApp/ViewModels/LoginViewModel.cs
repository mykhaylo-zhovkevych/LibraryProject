using CommunityToolkit.Mvvm.Input;
using LibraryProject.Presentation.DesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public LoginViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }
        public LoginViewModel()
        {
            // designer / previewer
        }

        [RelayCommand]
        private async Task NavigateToRegister()
        {
            await _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
