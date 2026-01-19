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

        public CatalogViewModel(ItemService itemService)
        {
            _itemService = itemService;
            PageName = ApplicationPageNames.Catalog;

            SelectedFilterOption = FilterOptions[0];
            _ = LoadDataAsync();
        }

        partial void OnSearchTextChanged(string? value) => DebouncedReload();
        partial void OnSelectedFilterOptionChanged(string? value) => DebouncedReload();

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
                    await Task.Delay(1500, ct); // wait untill userd typed a bit 
                    await LoadDataAsync();
                }
                catch (TaskCanceledException)
                {
                    // ignore
                }
            }, ct);
        }

        [RelayCommand]
        private async Task OpenConfirmDialogAsync()
        {
            var dialog = new ConfirmDialogViewModel
            {
                Title = "Bestätigung",
                Message = "Möchten Sie diese Aktion wirklich durchführen?",
                ConfirmText = "Ja",
                CancelText = "Nein"
            };

            CurrentDialog = dialog;
            dialog.Show();

            await dialog.WaitDialogAsnyc();

            if (dialog.Confirmed)
            {
                // Do something
            }
        }


        private async Task LoadDataAsync(CancellationToken ct = default)
        {
            (bool? isBorrowed, bool? isReserved) cases = SelectedFilterOption switch
            {
                "Alle" => (null, null),
                "Verfuegbar" => (false, false),
                "Reserviert" => (false, true),
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

        private int CalculateTotalCopies(Item item)
        {
            return item.CirculationCount;
        }

        private async Task<int> CalculateAvailableCopiesAsync(Item item)
        {
            IEnumerable<Item> foundFirstItems = await _itemService.SearchForDesiredItem(
                nameContains: item.Name,
                yearSelected: item.Year,
                itemType: item.ItemType);

            List<Item> allFoundItems = foundFirstItems.ToList();

            return allFoundItems.Count(i => i.IsBorrowed == false && i.IsReserved == false);
        }

        private int GetTotalFoundItems()
        {
            return Items.Count();
        }

    }
}
