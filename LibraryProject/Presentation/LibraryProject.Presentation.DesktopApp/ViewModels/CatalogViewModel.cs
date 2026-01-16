using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class CatalogViewModel : PageViewModel
    {
        private readonly ItemService _itemService;

        public ObservableCollection<DisplayedItem> Items { get; } = new();
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

        partial void OnSearchTextChanged(string? value)
        {
            throw new NotImplementedException();
        }

        partial void OnSelectedFilterOptionChanged(string? value)
        {
            throw new NotImplementedException();
        }


        public CatalogViewModel(ItemService itemService)
        {
            _itemService = itemService;
            PageName = ApplicationPageNames.Catalog;

            SelectedFilterOption = FilterOptions[0];

            LoadDataAsync();
            AddFakeData();
        }

        private async Task LoadDataAsync()
        {
            (bool? isBorrowed, bool? isReserved) cases = SelectedFilterOption switch
            {
                "Alle" => (null, null),
                "Verfuegbar" => (false, false),
                "Reserviert" => (false, true),
                "Ausgeliehen" => (true, null),
                _ => throw new NotImplementedException(),
            };

            var selectedItems = await _itemService.SearchForDesiredItem(nameContains: SearchText, cases.isBorrowed, cases.isReserved);

            Items.Clear();
            foreach (var i in selectedItems)
            {
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
                availableCopies: await CalculateAvailableCopies(item),
                totalCopies: await CalculateTotalCopies(item)
            );
        }

        private void AddFakeData()
        {
            Items.Add(new DisplayedItem(
                Guid.NewGuid(),
                "Die Verwandelung",
                "Kafka",
                "Lorem ipesuim wefwef",
                1954,
                "Genre",
                5,
                10
            ));
        }

        private async Task<int> CalculateTotalCopies(Item item)
        {
            //
            IEnumerable<Item> foundFirstItems = await _itemService.SearchForDesiredItem(nameContains: item.Name, yearSelected: item.Year, itemType: item.ItemType);

            var allFoundItems = foundFirstItems.ToList();

            if (allFoundItems.Count() == item.CirculationCount)
            {
                return allFoundItems.Count();
            }
            else if (allFoundItems.Count() > item.CirculationCount || allFoundItems.Count() < item.CirculationCount)
            {
                throw new ArgumentException();
            }

            else return 0;
        }

        private async Task<int> CalculateAvailableCopies(Item item)
        {
            IEnumerable<Item> foundFirstItems = await _itemService.SearchForDesiredItem(
                nameContains: item.Name,
                yearSelected: item.Year,
                itemType: item.ItemType);

            List<Item> allFoundItems = foundFirstItems.ToList();

            return allFoundItems.Count(i => i.IsBorrowed == false && i.IsReserved == false);
        }
    }
}
