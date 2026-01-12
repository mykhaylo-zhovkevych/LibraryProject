using CommunityToolkit.Mvvm.Input;
using LibraryProject.Presentation.DesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class RegisterViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;

        public RegisterViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public RegisterViewModel() : this(null!) { }


        [RelayCommand]
        private async Task NavigateToLogin()
        {
            await _navigationService.NavigateTo<LoginViewModel>();
        }
    }
}
