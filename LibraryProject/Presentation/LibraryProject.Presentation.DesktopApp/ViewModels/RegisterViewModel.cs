using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
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

        //[ObservableProperty] private string? _userId;

        [ObservableProperty] private string? _name;
        [ObservableProperty] private string? _surname;
        [ObservableProperty] private string? _address;

        [ObservableProperty] private string? _accountName;
        [ObservableProperty] private string? _email;
        [ObservableProperty] private string? _password;

        [ObservableProperty] private string? _errorMessage;
        [ObservableProperty] private string? _TempUserId;

        [ObservableProperty] private bool? _showTempUserId;

        //partial void OnShowTempUserIdChanged(bool? value)
        //{
        //    TempUserId = value == true ? UserId : null;
        //}

        [RelayCommand]
        public async Task TryToRegisterAsync()
        {
            ErrorMessage = null;
            TempUserId = null;
            try
            {
                Account result = await _accountService.RegisterAdminAccountAsync(AccountName, Password, Email, Name, Surname, Address, default);

                if (result != null)
                {
                    //TempUserId = result.UserId.ToString();
                    //await Task.Delay(5000);
                    //TempUserId = null;

                    await _navigationService.NavigateTo<LoginViewModel>();
                    return;
                }
                else
                {
                    ErrorMessage = "Registrierung fehlgeschlagen. Bitte versuchen Sie es erneut.";
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
