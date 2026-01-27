using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class ProfileViewModel : PageViewModel
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly UserService _userService;
        private readonly AccountService _accountService;
        private bool _initialized;

        [ObservableProperty]
        private string _accountName;

        [ObservableProperty]
        private string _userType;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _isSuspended;


        [ObservableProperty]
        private bool _isEditingEmail;
        [ObservableProperty]
        private string _emailDraft;

        public ProfileViewModel(ICurrentUserContext currentUserContext, UserService userService, AccountService accountService)
        {
            _currentUserContext = currentUserContext;
            _userService = userService;
            _accountService = accountService;
            PageName = ApplicationPageNames.Profile;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            _initialized = true;

            await LoadDataAsync();
        }

        private async Task LoadDataAsync(CancellationToken ct = default)
        {
            User? currentUser = await _userService.ReceiveUserByIdAsync(_currentUserContext.UserId.Value, ct);
            Account? currentAccount = await _accountService.ReceiveAccountByUserIdAsync(_currentUserContext.UserId.Value, ct);

            AccountName = currentAccount.AccountName;
            UserType = currentUser.UserType.ToString();
            IsSuspended = currentAccount.IsSuspended ? "Ja" : "Nein";
            Email = currentAccount.Email ?? "Keine E-Mail";

            EmailDraft = Email;
            IsEditingEmail = false;
        }

        [RelayCommand]
        private void BeginEditEmail()
        {
            EmailDraft = Email;
            IsEditingEmail = true;
        }


        [RelayCommand]
        private void CancelEditEmail()
        {
            EmailDraft = Email;
            IsEditingEmail = false;
        }

        [RelayCommand]
        private async Task SaveEmailAsync()
        {
            if (!string.IsNullOrWhiteSpace(EmailDraft))
            {
                await _accountService.UpdateEmailAsync(Email);
                Email = EmailDraft;
            }  
            IsEditingEmail = false;
        }
    }
}