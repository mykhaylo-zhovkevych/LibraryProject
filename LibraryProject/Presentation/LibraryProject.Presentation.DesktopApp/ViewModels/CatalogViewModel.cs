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
        private CancellationTokenSource? _reloadCts;

        private const string FilterAll = "Alle";
        private const string FilterAvailable = "Verfuegbar";
        private const string FilterReserved = "Reserviert";
        private const string FilterBorrowed = "Ausgeliehen";

        public ObservableCollection<DisplayedItem> Items { get; set; } = new();

        public int TotalFoundItems => Items.Count;
        public ObservableCollection<string> FilterOptions { get; } = new()
        {
            FilterAll,
            FilterAvailable,
            FilterReserved,
            FilterBorrowed
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

        }

        public async Task InitializedAsync()
        {
            if (_initialized) return;
            _initialized = true;

            SelectedFilterOption = FilterOptions[0];

            await LoadDataAsync(CancellationToken.None);
        }

        partial void OnSearchTextChanged(string? value)
        {
            if (!_initialized) return;
            DebouncedReload();
        }

        partial void OnSelectedFilterOptionChanged(string? value)
        {
            if (!_initialized) return;
            DebouncedReload();
        }

        private void DebouncedReload()
        {
            _reloadCts?.Cancel();
            _reloadCts?.Dispose();
            _reloadCts = new CancellationTokenSource();

            CancellationToken ct = _reloadCts.Token;

            _ = DebouncedReloadCoreAsync(ct);
        }

        private async Task DebouncedReloadCoreAsync(CancellationToken ct)
        {
            try
            {
                await Task.Delay(1500, ct);
                await LoadDataAsync(ct);
            }
            catch (Exception ex)
            {
            }
        }

        private int CalculateAvailableCopiesAsync(Item item) => item.CirculationCount;

        private (bool? isBorrowed, bool? isReserved) GetFilterCases()
        {
            return SelectedFilterOption switch
            {
                FilterAll => (null, null),
                FilterAvailable => (false, false),
                FilterReserved => (null, true),
                FilterBorrowed => (true, null),
                _ => (null, null),
            };
        }

        private async Task LoadDataAsync(CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();

            (bool? isBorrowed, bool? isReserved) cases = GetFilterCases();

            IEnumerable<Item> selectedItems = await _itemService.SearchForDesiredItem(nameContains: SearchText, isBorrowed: cases.isBorrowed, isReserved: cases.isReserved
            );

            ct.ThrowIfCancellationRequested();

            Items.Clear();

            foreach (Item i in selectedItems)
            {
                ct.ThrowIfCancellationRequested();
                Items.Add(MapItemToDisplayedItem(i));
            }

            OnPropertyChanged(nameof(TotalFoundItems));
        }

        private ArchiveStatus CalculateArchiveStatus(Item item)
        {
            if (item.IsArchived)
            {
                return ArchiveStatus.Yes;
            }
            int total = item.Copies?.Count ?? 0;
            if (total == 0)
            {
                return ArchiveStatus.No;
            }

            int archived = item.Copies.Count(c => c.IsArchived);

            if (archived == 0) return ArchiveStatus.No;
            if (archived == total) return ArchiveStatus.Yes;

            return ArchiveStatus.Partial;
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
                availableCopies: CalculateAvailableCopiesAsync(item),
                archiveStatus: CalculateArchiveStatus(item)
            );
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
                    User currentUser = await _userService.ReceiveUserByIdAsync(userId, default) ?? throw new InvalidOperationException("Angemeldeter Benutzer nicht gefunden.");

                    Item domainItem = (await _itemService.SearchForDesiredItem(nameContains: item.Title,
                        yearSelected: item.Year,
                        itemType: null,
                        customPredicate: i => i.Author == item.Author)).FirstOrDefault() ?? throw new InvalidOperationException("Artikel nicht gefunden.");

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
                    User currentUser = await _userService.ReceiveUserByIdAsync(userId, default) ?? throw new InvalidOperationException("Angemeldeter Benutzer nicht gefunden.");

                    Item domainItem = (await _itemService.SearchForDesiredItem(nameContains: item.Title,
                        yearSelected: item.Year,
                        itemType: null,
                        customPredicate: i => i.Author == item.Author)).FirstOrDefault() ?? throw new InvalidOperationException("Artikel nicht gefunden.");

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
    }
}