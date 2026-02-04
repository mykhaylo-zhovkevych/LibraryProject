using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Models;
using LibraryProject.Presentation.DesktopApp.ViewModels.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels
{
    public partial class UsersViewModel : PageViewModel
    {
        private readonly UserService _userService;
        private readonly AccountService _accountService;
        private readonly BorrowingService _borrowingService;
        private bool _initialized;

        public ObservableCollection<DisplayedUser> Users { get; } = new();
        public ObservableCollection<DisplayedAccount> Accounts { get; } = new();

        [ObservableProperty] private string _searchText = "";

        [ObservableProperty] private DisplayedUser? _selectedUser;
        [ObservableProperty] private DisplayedAccount? _selectedAccount;

        public bool HasSelectedUser => SelectedUser != null;
        public bool HasSelectedAccount => SelectedAccount != null;



        public UsersViewModel(UserService userService, AccountService accountService, BorrowingService borrowingService)
        {
            _accountService = accountService;
            _userService = userService;
            _borrowingService = borrowingService;

            PageName = ApplicationPageNames.ManagementUsers;
        }


        partial void OnSelectedUserChanged(DisplayedUser? value) => OnPropertyChanged(nameof(HasSelectedUser));

        partial void OnSelectedAccountChanged(DisplayedAccount? value) => OnPropertyChanged(nameof(HasSelectedAccount));


        public async Task InitializeAsync()
        {
            if (_initialized) return;
            _initialized = true;

            await ReloadAsync();
        }

        [RelayCommand]
        private async Task ReloadAsync()
        {
            try
            {
                await LoadUsersAndAccountsAsync();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }


        [RelayCommand]
        private async Task LockAccountAsync()
        {
            if (SelectedAccount == null)
            {
                ShowError("Kein Account ausgewählt.");
                return;
            }

            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel
            {
                Title = "Account sperren",
                Message = $"Möchten Sie den Account {SelectedAccount.AccountName} wirklich sperren?",
                ConfirmText = "Sperren",
                CancelText = "Abbrechen"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    await _accountService.SuspendAccountAsync(SelectedAccount.AccountId, default);
                    await ReloadAsync();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }


        [RelayCommand]
        private async Task UnlockAccountAsync()
        {
            if (SelectedAccount == null)
            {
                ShowError("Kein Account ausgewählt.");
                return;
            }

            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel
            {
                Title = "Account entsperren",
                Message = $"Möchten Sie den Account {SelectedAccount.AccountName} wirklich entsperren?",
                ConfirmText = "Entsperren",
                CancelText = "Abbrechen"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    await _accountService.ReactivateAccountAsync(SelectedAccount.AccountId, default);
                    await ReloadAsync();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }    
        }


        [RelayCommand]
        private async Task DeleteAccountAsync()
        {
            if (SelectedAccount == null)
            {
                ShowError("Kein Account ausgewählt.");
                return;
            }

            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel
            {
                Title = "Account löschen",
                Message = $"Möchten Sie den Account {SelectedAccount.AccountName} wirklich löschen?",
                ConfirmText = "Löschen",
                CancelText = "Abbrechen"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    await _accountService.DeleteAccountAsync(SelectedAccount.AccountId, SelectedAccount.UserId, default);

                    await ReloadAsync();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }   
        }

        [RelayCommand]
        private async Task DeleteUserAsync()
        {
            if (SelectedUser == null)
            {
                ShowError("Kein Benutzer ausgewählt.");
                return;
            }

            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel
            {
                Title = "Kunde löschen",
                Message = $"Möchten Sie den Benutzer {SelectedUser.Name} wirklich löschen?",
                ConfirmText = "Löschen",
                CancelText = "Abbrechen"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    await _userService.DeleteExistingUserAsync(SelectedUser.Id, default);
                    await ReloadAsync();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }    
        }

        //[RelayCommand]
        //private async Task ConfirmIdentityAsync()
        //{
        //    if (SelectedUser == null)
        //    {
        //        ShowError("Kein Benutzer ausgewählt.");
        //        return;
        //    }

        //    ConfirmDialogViewModel dialog = new ConfirmDialogViewModel
        //    {
        //        Title = "Identität bestätigen",
        //        Message = $"Möchten Sie die Identität von {SelectedUser.Name} bestätigen?",
        //        ConfirmText = "Bestätigen",
        //        CancelText = "Abbrechen"
        //    };

        //    CurrentDialog = dialog;
        //    dialog.Show();

        //    if (await dialog.WaitConfirmationAsync())
        //    {
        //        try
        //        {
        //            await _userService.ConfirmIdentityAsync(SelectedUser.Id, default);
        //            await ReloadAsync();
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowError(ex.Message);
        //        }
        //    }    
        //}

        [RelayCommand]
        private async Task ShowBorrowingHistoryAsync()
        {
            if (SelectedUser == null)
            {
                ShowError("Kein Benutzer ausgewählt.");
                return;
            }

            try
            {
                List<Borrowing> history =  await _borrowingService.SearchAllBorrowingsByUserId(SelectedUser.Id, default);

                if (history.Count == 0)
                {
                    ShowError("Keine Ausleihen für diesen Benutzer gefunden.");
                    return;
                }

                BorrowingHistoryDialogViewModel dialog = new BorrowingHistoryDialogViewModel(history)
                {
                    Title = "Ausleihhistorie",
                    Message = $"Hier sind alle Ausleihen von {SelectedUser.Name}.",
                    ConfirmText = "Bestätigen",
                    CancelText = "Abbrechen"
                };

                CurrentDialog = dialog;
                dialog.Show();

                await dialog.WaitDialogAsnyc();
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        private async Task LoadUsersAndAccountsAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            List<User> users = await _userService.ReceiveAllUsersAsync(ct);
            List<Account> accounts = await _accountService.ReceiveAllAccountsAsync(ct);

            Users.Clear();
            foreach (User u in users.OrderBy(x => x.Name))
            {
                Users.Add(new DisplayedUser(
                    id: u.Id,
                    name: u.Name,
                    userType: u.UserType.ToString()
                ));
            }

            Accounts.Clear();
            foreach (Account a in accounts.OrderBy(x => x.AccountName))
            {
                Accounts.Add(new DisplayedAccount(
                    accountId: a.AccountId,
                    userId: a.UserId,
                    accountName: a.AccountName,
                    email: a.Email ?? "",
                    status: a.IsSuspended ? "Gesperrt" : "Aktiv"
                ));
            }

            // Reset selections
            SelectedUser = null;
            SelectedAccount = null;

            OnPropertyChanged(nameof(HasSelectedUser));
            OnPropertyChanged(nameof(HasSelectedAccount));
        }

        private void ShowError(string message)
        {
            CurrentDialog = new ErrorDialogViewModel
            {
                Title = "Fehler",
                Message = message,
                ConfirmText = "OK"
            };
            CurrentDialog.Show();
        }
    }
}
