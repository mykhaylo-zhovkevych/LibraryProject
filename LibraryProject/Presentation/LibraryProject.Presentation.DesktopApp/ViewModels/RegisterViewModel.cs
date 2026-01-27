using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
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
        private readonly AccountService _accountService;


        public RegisterViewModel(INavigationService navigationService, AccountService accountService)
        {
            _navigationService = navigationService;
            _accountService = accountService;
        }

        [ObservableProperty] private string? _userId;
        [ObservableProperty] private string? _name;
        [ObservableProperty] private string? _email;
        [ObservableProperty] private string? _password;

        [ObservableProperty] private string? _errorMessage;


        [RelayCommand]
        public async Task TryToRegisterAsync()
        {
            ErrorMessage = null;

            Guid userGuidId;

            if (!Guid.TryParse(UserId, out userGuidId))
            {
                ErrorMessage = "Invalid User ID format.";
                return;
            }

            try
            {
                var result = await _accountService.RegisterAccountAsync(userGuidId, Name, Password, Email, default);

                if (result != null)
                {
                    await Task.Delay(3000);
                    await _navigationService.NavigateTo<LoginViewModel>();
                    return;
                }
                else
                {
                    ErrorMessage = "Registration failed. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"{ex.Message}";
            }
        }

        [RelayCommand]
        public async Task NavigateToLogin()
        {
            await _navigationService.NavigateTo<LoginViewModel>();
        }
    }
}
