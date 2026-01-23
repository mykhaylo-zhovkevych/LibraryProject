using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Models;
using LibraryProject.Presentation.DesktopApp.ViewModels.Dialog;
using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels
{
    public partial class ItemsViewModel : PageViewModel
    {
        private readonly ItemService _itemService;
        private readonly UserService _userService;
        private readonly ICurrentUserContext _currentUserContext;

        public ObservableCollection<DisplayedItem> Items { get; } = new();

        public ItemsViewModel(ItemService itemService, ICurrentUserContext currentUserContext, UserService userService)
        {
            _itemService = itemService;
            _currentUserContext = currentUserContext;
            _userService = userService;
            PageName = ApplicationPageNames.ManagementItems;

            _ = LoadItemsAsync();
        }

        [ObservableProperty]
        private DisplayedItem? _selectedItem;

        // TODO: Add the popup with the update item
        [RelayCommand]
        private Task Reload() => LoadItemsAsync();

        [RelayCommand]
        private async Task ShowDeleteItemDialog()
        {
            if (SelectedItem == null)
            {
                CurrentDialog = new ErrorDialogViewModel()
                {
                    Title = "Fehler",
                    Message = "Kein Element ausgewählt.",
                    ConfirmText = "OK"
                };
                CurrentDialog.Show();
                return;
            }

            DisplayedItem selectedItem = SelectedItem;

            DeleteItemDialogViewModel dialog = new DeleteItemDialogViewModel()
            {
                Title = "Löschen bestätigen",
                Message = $"Möchten Sie “{selectedItem.Title}“ löschen?",
                ConfirmText = "Ja",
                CancelText = "Nein"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if ( await dialog.WaitConfirmationAsync())
            {
                try
                {

                    Guid userId = _currentUserContext.UserId.Value;
                    User currentUser = await _userService.ReceiveUserByIdAsync(userId, default) ?? throw new InvalidOperationException("Logged-in user not found.");

                    Item domainItem = (await _itemService.SearchForDesiredItem(nameContains: selectedItem.Title,
                        yearSelected: selectedItem.Year,
                        itemType: null,
                        customPredicate: i => i.Author == selectedItem.Author)).FirstOrDefault() ?? throw new InvalidOperationException("Item not found.");

                    if (dialog.DeleteAllCopies)
                    {
                        await _itemService.RemoveItemAsync(domainItem, default);
                    }
                    else
                    {
                        await _itemService.RemoveItemCopiesByIdAsync(domainItem, dialog.CountToDelete, default);
                    }

                    LoadItemsAsync();

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

        private async Task LoadItemsAsync(CancellationToken ct = default)
        {
            IEnumerable items = await _itemService.SearchForDesiredItem();

            Items.Clear();
            foreach (Item item in items)
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
