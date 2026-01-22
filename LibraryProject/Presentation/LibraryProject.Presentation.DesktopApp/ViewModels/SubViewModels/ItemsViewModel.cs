using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Models;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels
{
    public partial class ItemsViewModel : PageViewModel
    {
        private readonly ItemService _itemService;

        public ObservableCollection<DisplayedItem> Items { get; } = new();

        public ItemsViewModel(ItemService itemService)
        {
            _itemService = itemService;
            PageName = ApplicationPageNames.ManagementItems;

            _ = LoadItemsAsync();
        }

        [RelayCommand]
        private Task Reload() => LoadItemsAsync();

        private async Task LoadItemsAsync(CancellationToken ct = default)
        {
            var items = await _itemService.SearchForDesiredItem();

            Items.Clear();
            foreach (var item in items)
            {
                ct.ThrowIfCancellationRequested();
                Items.Add(MapItemToDisplayedItem(item));
            }
        }

        private static DisplayedItem MapItemToDisplayedItem(Item item)
        {
            return new DisplayedItem(
                item.Id,
                item.Name,
                item.Author,
                item.Description ?? string.Empty,
                item.Year,
                item.ItemType.ToString(),
                availableCopies: item.CirculationCount
            );
        }
    }
}
