using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Presentation.DesktopApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class LoginViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly AccountService _accountService;
        private readonly ICurrentUserContext _currentUser;

        public LoginViewModel(INavigationService navigationService, AccountService accountService, ICurrentUserContext currentUser)
        {
            _navigationService = navigationService;
            _accountService = accountService;
            _currentUser = currentUser;
        }
        public LoginViewModel()
        {
            // designer / previewer
        }


        public async Task Login(CancellationToken ct = default)
        {
            // Data validation

            // Data setting
            var session = await _accountService.LoginAsync(userId, name, password, ct);

            _currentUser.SignIn(session.UserId, session.UserType);

            // Navigation
        }

        public void Logout()
        {
            _currentUser.SignOut();
        }

        [RelayCommand]
        private async Task NavigateToRegister()
        {
            await _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
