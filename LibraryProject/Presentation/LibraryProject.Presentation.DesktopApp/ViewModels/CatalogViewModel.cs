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

            _ = LoadDataAsync();
        }

        partial void OnSearchTextChanged(string? value) => DebouncedReload();

        partial void OnSelectedFilterOptionChanged(string? value)
        {
            if (_suppressReload)
                return;

            DebouncedReload();
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
        private async Task ShowItemDialogAsync(DisplayedItem item)
        {
            BorrowDialogViewModel dialog = new BorrowDialogViewModel()
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
                    User user = await _userService.GetUserByIdAsync(userId, default) ?? throw new InvalidOperationException("Logged-in user not found.");

                    // Get genric item 
                    Item domainItem = (await _itemService.SearchForDesiredItem(nameContains: item.Title, yearSelected: item.Year, itemType: null, customPredicate: i => i.Author == item.Author)).FirstOrDefault() ?? throw new InvalidOperationException("Item not found.");

                    // Find copies
                    ItemCopy freeCopy = domainItem.Copies.FirstOrDefault(c => !c.IsBorrowed && c.ReservedById == null) ?? throw new InvalidOperationException("No copies of the item found.");

                    await _borrowingService.CreateBorrowedItemAsync(user, domainItem, default);

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
                finally
                {
                    // CurrentDialog = null;
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

            var selectedItems = await _itemService.SearchForDesiredItem(nameContains: SearchText, isBorrowed: cases.isBorrowed, isReserved: cases.isReserved);

            Items.Clear();
            foreach (var i in selectedItems)
            {
                ct.ThrowIfCancellationRequested();
                Items.Add(await MapItemToDisplayedItem(i));
            }

            OnPropertyChanged(nameof(TotalFoundItems));
        }

        private async Task<DisplayedItem> MapItemToDisplayedItem(Item item)
        {
            return new DisplayedItem(
                item.Id,
                item.Name,
                item.Author,
                item.Description ?? string.Empty,
                item.Year,
                item.ItemType.ToString(),
                availableCopies: await CalculateAvailableCopiesAsync(item),
                totalCopies: CalculateTotalCopies(item)
            );
        }

        private int CalculateTotalCopies(Item item) => item.CirculationCount;


        private async Task<int> CalculateAvailableCopiesAsync(Item item)
        {
            var items = await _itemService.SearchForDesiredItem(
                nameContains: item.Name,
                yearSelected: item.Year,
                itemType: item.ItemType,
                customPredicate: i => i.Author == item.Author);

            Item? domainItem = items.FirstOrDefault();
            if (domainItem == null || domainItem.Copies == null)
            {
                return 0;
            }
                
            return domainItem.Copies.Count(c => !c.IsBorrowed && c.ReservedById == null);
        }


        private int GetTotalFoundItems()
        {
            return Items.Count();
        }

    }
}
