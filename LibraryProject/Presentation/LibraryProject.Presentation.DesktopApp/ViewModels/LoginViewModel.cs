using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Dto;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Presentation.DesktopApp.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        //[ObservableProperty] private string? _userId;
        [ObservableProperty] private string? _accountName;
        [ObservableProperty] private string? _password;

        [ObservableProperty] private string? _errorMessage;


        [RelayCommand]
        public async Task TryToLoginAsync()
        {
            ErrorMessage = null;

            //Guid userguidId;

            //if (!Guid.TryParse(UserId, out userguidId))
            //{
            //    ErrorMessage = "Login failed. Please check your ID.";
            //    return;
            //}
            
            try
            {
                LoginSession? session = await _accountService.LoginAdminAsync(AccountName, Password, default);

                if (session != null)
                {
                    _currentUser.SignIn(session.UserId, session.UserType);

                    await _navigationService.NavigateTo<DashboardViewModel>();
                }
                else
                {
                    ErrorMessage = "Anmeldung fehlgeschlagen. Bitte überprüfen Sie Ihre Anmeldeinformationen.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex.Message}";
            }
        }

        [RelayCommand]
        public async Task NavigateToRegister()
        {
            await _navigationService.NavigateTo<RegisterViewModel>();
        }
    }
}
