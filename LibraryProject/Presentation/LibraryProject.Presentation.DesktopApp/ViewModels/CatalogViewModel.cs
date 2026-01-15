using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using LibraryProject.Application.Interfaces;
using LibraryProject.Presentation.DesktopApp.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class CatalogViewModel : PageViewModel
    {
        private readonly IItemRepository _itemRepository;


        [ObservableProperty]
        private List<string> _allItems;

        [ObservableProperty]
        private List<string> _filterOptions;
        [ObservableProperty]
        private string? _selectedFilterOption;


        public CatalogViewModel(IItemRepository itemRepository)
        {
            _itemRepository = itemRepository;
            PageName = ApplicationPageNames.Catalog;


            FilterOptions =
            [
                "Alle",
                "Nur verfügbar",
                "Reserviert",
                "Neu"
            ];

            SelectedFilterOption = FilterOptions[0];

            AllItems =
            [
                    @"C:\item\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",
                    @"C:\item\local\etc",

            ];
        }




    }
}
