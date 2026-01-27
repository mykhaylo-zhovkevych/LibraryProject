using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
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
        private bool _initialized;

        [ObservableProperty] private string _newTitle = "";
        [ObservableProperty] private string _newAuthor = "";
        [ObservableProperty] private string _newYearText = "";
        [ObservableProperty] private string _newCopiesText = "";
        [ObservableProperty] private string _newDescription = "";

        [ObservableProperty]
        private DisplayedItem? _selectedItem;

        [ObservableProperty]
        private ItemType _selectedItemType = ItemType.Book;

        [ObservableProperty]
        private string _newShelfIdText = "";

        public ObservableCollection<ItemType> ItemTypes { get; } = new ObservableCollection<ItemType>(Enum.GetValues<ItemType>());
        public ObservableCollection<DisplayedItem> Items { get; } = new();

        public int? NewShelfId => int.TryParse(NewShelfIdText, out var id) ? id : null;
        public int? NewYear => int.TryParse(NewYearText, out var y) ? y : 0;
        public int? NewCopies => int.TryParse(NewCopiesText, out var c) ? c : 1;

        public ItemsViewModel(ItemService itemService, ICurrentUserContext currentUserContext, UserService userService)
        {
            _itemService = itemService;
            _currentUserContext = currentUserContext;
            _userService = userService;

            PageName = ApplicationPageNames.ManagementItems;
        }

        public async Task InitializeAsync()
        {
            if (_initialized) return;
            _initialized = true;

            await LoadItemsAsync();
        }

        [RelayCommand]
        private async Task CreateNewItem()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewTitle))
                {
                    throw new ArgumentException("Titel ist leer.");
                }

                if (string.IsNullOrWhiteSpace(NewAuthor))
                {
                    throw new ArgumentException("Autor ist leer.");
                }

                if (NewYear is null)
                {
                    throw new ArgumentException("Year ist ungültig.");
                }
                    
                if (NewCopies is null || NewCopies.Value <= 0)
                {
                    throw new ArgumentException("Copies ist ungültig.");
                }

                await _itemService.CreateItemWithAmount(
                    NewTitle.Trim(), 
                    SelectedItemType, 
                    NewAuthor.Trim(), 
                    NewYear.Value, 
                    string.IsNullOrWhiteSpace(NewDescription) ? null : NewDescription.Trim(), 
                    NewCopies.Value, 
                    NewShelfId, 
                    default);

                ResetCreateForm();
                await LoadItemsAsync();
            }
            catch (Exception ex)
            {
                CurrentDialog = new ErrorDialogViewModel
                {
                    Title = "Fehler",
                    Message = ex.Message,
                    ConfirmText = "OK"
                };
                CurrentDialog.Show();
            }
        }

        [RelayCommand]
        private void CancelCreate() => ResetCreateForm();

        private void ResetCreateForm()
        {
            NewTitle = "";
            NewAuthor = "";
            NewYearText = "";
            NewCopiesText = "";
            NewDescription = "";
            SelectedItemType = ItemType.Book;
        }

        [RelayCommand]
        private async Task ShowUpdateItemDialog()
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

            var selectedItem = SelectedItem;

            var dialog = new UpdateItemDialogViewModel(selectedItem)
            {
                Title = "Artikel bearbeiten",
                Message = $"Bearbeiten Sie “{selectedItem.Title}” und speichern Sie die Änderungen.",
                ConfirmText = "Speichern",
                CancelText = "Abbrechen"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    await _itemService.UpdateItemAsync(
                        itemId: selectedItem.Id,
                        title: dialog.ItemTitle,
                        author: dialog.Author,
                        year: dialog.Year.Value,
                        description: dialog.Description,
                        ct: default);

                    await LoadItemsAsync();
                }
                catch (Exception ex)
                {
                    CurrentDialog = new ErrorDialogViewModel()
                    {
                        Title = "Fehler",
                        Message = $"Fehler: {ex.Message}",
                        ConfirmText = "OK"
                    };
                    CurrentDialog.Show();
                }
            }   
        }


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

        [RelayCommand]
        private async Task ShowAddExtraCopiesDialog()
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

            AddExtraItemDialogViewModel dialog = new AddExtraItemDialogViewModel()
            {
                Title = "Kopien hinzufügen",
                Message = $"Wie viele zusätzliche Exemplare möchten Sie für {selectedItem.Title} hinzufügen?",
                ConfirmText = "Hinzufügen",
                CancelText = "Abbrechen",
                CountToAdd = 1
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    await _itemService.AddCopiesToItemAsync(selectedItem.Id, dialog.CountToAdd, default);
                    await LoadItemsAsync();
                }
                catch (Exception ex)
                {
                    CurrentDialog = new ErrorDialogViewModel()
                    {
                        Title = "Fehler",
                        Message = $"Fehler: {ex.Message}",
                        ConfirmText = "OK"
                    };
                    CurrentDialog.Show();
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
