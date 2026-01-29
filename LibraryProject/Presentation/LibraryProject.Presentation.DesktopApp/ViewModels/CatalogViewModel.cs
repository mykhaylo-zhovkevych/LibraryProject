using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.ViewModels.Dialog;
using LibraryProject.Presentation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class CatalogViewModel : PageViewModel
    {
        private readonly ItemService _itemService;
        private readonly BorrowingService _borrowingService;
        private readonly UserService _userService;
        private readonly ICurrentUserContext _currentUserContext;
        private bool _initialized;

        public ObservableCollection<DisplayedItem> Items { get; set; } = new();
        
        public int TotalFoundItems => GetTotalFoundItems();
        public ObservableCollection<string> FilterOptions { get; } = new()
        {
            "Alle",
            "Verfuegbar",
            "Reserviert",
            "Ausgeliehen"
        };

        [ObservableProperty]
        private string? _selectedFilterOption;

        [ObservableProperty]
        private string? _searchText;

        private CancellationTokenSource? _loadCts;
        private bool _suppressReload;

        public CatalogViewModel(ItemService itemService, 
                                BorrowingService borrowingService, 
                                UserService userService, 
                                ICurrentUserContext currentUserContext)
        {
            _itemService = itemService;
            _borrowingService = borrowingService;
            _userService = userService;
            _currentUserContext = currentUserContext;

            PageName = ApplicationPageNames.Catalog;

            _suppressReload = true;
            SelectedFilterOption = FilterOptions[0];
            _suppressReload = false;
        }

        partial void OnSearchTextChanged(string? value) => DebouncedReload();

        partial void OnSelectedFilterOptionChanged(string? value)
        {
            if (_suppressReload)
                return;

            DebouncedReload();
        }

        public async Task InitializedAsync()
        {
            if (_initialized) return;
            _initialized = true;

            await LoadDataAsync();
        }


        private void DebouncedReload()
        {
            _loadCts?.Cancel();
            _loadCts?.Dispose();

            _loadCts = new CancellationTokenSource();

            var ct = _loadCts.Token;

            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(1500, ct);
                    await LoadDataAsync();
                }
                catch (TaskCanceledException)
                {

                }
            }, ct);
        }

        [RelayCommand]
        private async Task ShowBorrowItemDialog(DisplayedItem item)
        {
            BorrowItemDialogViewModel dialog = new BorrowItemDialogViewModel()
            {
                Title = "Ausleihen bestätigen",
                Message = $"Möchten Sie “{item.Title}“ ausleihen?",
                ConfirmText = "Ja",
                CancelText = "Nein"
            };

            CurrentDialog = dialog;
            dialog.Show();


            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    Guid userId = _currentUserContext.UserId.Value;
                    User currentUser = await _userService.ReceiveUserByIdAsync(userId, default) ?? throw new InvalidOperationException("Logged-in user not found.");

                    Item domainItem = (await _itemService.SearchForDesiredItem(nameContains: item.Title,
                        yearSelected: item.Year,
                        itemType: null,
                        customPredicate: i => i.Author == item.Author)).FirstOrDefault() ?? throw new InvalidOperationException("Item not found.");

                    await _borrowingService.CreateBorrowedItemAsync(currentUser, domainItem, default);

                }
                catch (Exception ex)
                {
                    ErrorDialogViewModel errorDialog = new ErrorDialogViewModel()
                    {
                        Title = "Fehler",
                        Message = $"Fehler: {ex.Message}",
                        ConfirmText = "OK"
                    };
                    CurrentDialog = errorDialog;
                    errorDialog.Show();

                }
            }
        }


        [RelayCommand]
        private async Task ShowReserveItemDialog(DisplayedItem item)
        {
            ReserveItemDialogViewModel dialog = new ReserveItemDialogViewModel()
            {
                Title = "Reservation bestätigen",
                Message = $"Möchten Sie “{item.Title}“ reservieren?",
                ConfirmText = "Ja",
                CancelText = "Nein"
            };

            CurrentDialog = dialog;
            dialog.Show();


            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    Guid userId = _currentUserContext.UserId.Value;
                    User currentUser = await _userService.ReceiveUserByIdAsync(userId, default) ?? throw new InvalidOperationException("Logged-in user not found.");

                    Item domainItem = (await _itemService.SearchForDesiredItem(nameContains: item.Title,
                        yearSelected: item.Year,
                        itemType: null,
                        customPredicate: i => i.Author == item.Author)).FirstOrDefault() ?? throw new InvalidOperationException("Item not found.");

                    await _itemService.CreateReservedItemAsync(currentUser, domainItem, default);

                }
                catch (Exception ex)
                {
                    ErrorDialogViewModel errorDialog = new ErrorDialogViewModel()
                    {
                        Title = "Fehler",
                        Message = $"Fehler: {ex.Message}",
                        ConfirmText = "OK"
                    };
                    CurrentDialog = errorDialog;
                    errorDialog.Show();

                }
            }
        }


        private async Task LoadDataAsync(CancellationToken ct = default)
        {
            (bool? isBorrowed, bool? isReserved) cases = SelectedFilterOption switch
            {
                "Alle" => (null, null),
                "Verfuegbar" => (false, false),
                "Reserviert" => (null, true),
                "Ausgeliehen" => (true, null),
                _ => throw new NotImplementedException(),
            };

            IEnumerable<Item> selectedItems = await _itemService.SearchForDesiredItem(nameContains: SearchText, isBorrowed: cases.isBorrowed, isReserved: cases.isReserved);

            Items.Clear();
            foreach (Item i in selectedItems)
            {
                ct.ThrowIfCancellationRequested();
                Items.Add(MapItemToDisplayedItem(i));
            }

            OnPropertyChanged(nameof(TotalFoundItems));
        }

        private DisplayedItem MapItemToDisplayedItem(Item item)
        {
            return new DisplayedItem(
                item.Id,
                item.Name,
                item.Author,
                item.Description ?? string.Empty,
                item.Year,
                item.ItemType.ToString(),
                availableCopies: CalculateAvailableCopiesAsync(item)
            );
        }

        private int CalculateAvailableCopiesAsync(Item item) => item.CirculationCount;


        private int GetTotalFoundItems() => Items.Count();

    }
}
