using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
using LibraryProject.Domain.Exceptions.Nonexistent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services
{
    public class ItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IAuthorizationService _authorizationService;
        private const int DefaultShelfId = 101;

        public ItemService(IItemRepository itemfRepository, IAuthorizationService authorizationService)
        {
            _itemRepository = itemfRepository;
            _authorizationService = authorizationService;
        }

        public async Task CreateItemWithAmount(string name, ItemType itemType, string author, int year, string? description, int circulationCount, int? shelfId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Der Name des Mediums darf nicht leer sein.");
            }

            if (circulationCount < 0)
            {
                throw new ArgumentException("Die Anzahl der Exemplare muss größer als 0 sein.");
            }

            _authorizationService.EnsureAdmin();

            Item item = new Item(name.Trim(),itemType, author.Trim(), year, description: string.IsNullOrWhiteSpace(description) ? null : description.Trim(), 0);

            item.AddCopies(circulationCount);
            item.CirculationCount = circulationCount;

            await AddItemToShelf(item, shelfId, ct);
        }

        public async Task CreateReservedItemAsync(User user, Item item, CancellationToken ct)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "Benutzer darf nicht null sein.");
            if (item == null) throw new NonexistentItemException();

            ItemCopy? copyToReserve = await _itemRepository.GetCopyToReserveAsync(item.Id, ct);
            if (copyToReserve == null)
            {
                throw new ArgumentException($"Für {item.Name} kann kein Exemplar reserviert werden.");
            }

            if (!copyToReserve.CheckReservePossible())
            {
                throw new ItemUsedByException(item);
            }

            copyToReserve.ReserveItem(user);
            await _itemRepository.UpdateCopyAsync(copyToReserve, ct);
        }

        
        public async Task ArchiveItemAsync(Guid itemId, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            Item item = await _itemRepository.GetItemByIdAsync(itemId, ct) ?? throw new NonexistentItemException();

            item.ArchiveAllCopies(cancelReservations: true);

            await _itemRepository.UpdateItemAsync(item, ct);
        }

        public async Task ArchiveItemCopiesAsync(Guid itemId, int count, CancellationToken ct)
        {
            if (count <= 0) throw new ArgumentException("Ungültige Anzahl von Exemplaren.");
            _authorizationService.EnsureAdmin();


            Item item = await _itemRepository.GetItemByIdAsync(itemId, ct) ?? throw new NonexistentItemException();
            item.ArchiveSomeCopies(count);

            await _itemRepository.UpdateItemAsync(item, ct);
        }

        public async Task AddCopiesToItemAsync(Guid itemId, int count, CancellationToken ct)
        {
            if (count <= 0) throw new ArgumentException("Ungültige Anzahl von Exemplaren.");
            _authorizationService.EnsureAdmin();

            Item item = await _itemRepository.GetItemByIdAsync(itemId, ct) ?? throw new NonexistentItemException();

            if (item.IsArchived)
            {
                throw new InvalidOperationException("Zu archivierten Medien können keine Exemplare hinzugefügt werden.");
            }

            await _itemRepository.InsertCopiesToItemAsync(itemId, count, ct);
        }

        public async Task CancelReservation(User user, Item item)
        {
            if (user == null) throw new ArgumentNullException(nameof(user), "Benutzer darf nicht null sein.");
            if (item == null) throw new NonexistentItemException();

            _authorizationService.EnsureAuthenticated();

            ItemCopy? reservedCopy = item.Copies.FirstOrDefault(c => c.ReservedById == user.Id);
            if (reservedCopy == null)
            {
                throw new ArgumentException($"{user.Name} hat keine Reservierung für {item.Name}.");
            }

            reservedCopy.ReturnItem();
            await _itemRepository.UpdateCopyAsync(reservedCopy);

        }

        public async Task UpdateItemAsync(Guid itemId, string title, string author, int year, string? description, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Titel ist leer.");
            }
            if (string.IsNullOrWhiteSpace(author))
            {
                throw new ArgumentException("Autor ist leer.");
            }
            if (year <= 0)
            {
                throw new ArgumentException("Ungültiges Jahr.");
            }

            _authorizationService.EnsureAdmin();
            Item item = await _itemRepository.GetItemByIdAsync(itemId, ct) ?? throw new NonexistentItemException();

            item.UpdateItemName(title.Trim());
            item.UpdateAuthor(author.Trim());
            item.UpdateYear(year);
            item.UpdateDescription(string.IsNullOrWhiteSpace(description) ? null : description.Trim());

            await _itemRepository.UpdateItemAsync(item, ct);
        }

        public async Task<IEnumerable<Item>> SearchForDesiredItem(
            string? nameContains = null,
            bool? isBorrowed = null,
            bool? isReserved = null,
            int? yearSelected = null,
            ItemType? itemType = null,
            Func<Item, bool>? customPredicate = null
            )
        {
            IEnumerable<Item> items = await _itemRepository.GetAllItemsAsync();

            string term = nameContains?.Trim();

            if (!string.IsNullOrWhiteSpace(term))
            {
                items = items.Where(i => i.Name.Contains(term, StringComparison.OrdinalIgnoreCase));
            }

            if (isBorrowed.HasValue)
            {
                if (isBorrowed.Value)
                {
                    items = items.Where(i => i.Copies.Any(c => c.IsBorrowed));
                }
                else
                {
                    items = items.Where(i => i.Copies.Any(c => !c.IsBorrowed));
                }
            }

            if (isReserved.HasValue)
            {
                if (isReserved.Value)
                {
                    items = items.Where(i => i.Copies.Any(c => c.ReservedById != null));
                }
                else
                {
                    items = items.Where(i => i.Copies.Any(c => c.ReservedById == null));
                }
            }

            if (yearSelected.HasValue)
            {
                items = items.Where(i => i.Year == yearSelected.Value);
            }

            if (itemType != null)
            {
                items = items.Where(i => itemType.Equals(i.ItemType));
            }

            if (customPredicate != null)
            {
                items = items.Where(customPredicate);
            }
            return items.ToList();
        }

        private async Task<Item> AddItemToShelf(Item item, int? shelfId, CancellationToken ct)
        {
            Item? existingItem = await _itemRepository.GetExistingItemAsync(item.Name, item.ItemType, ct);
            if (existingItem != null)
            {
                throw new ItemAlreadyExistsWithThisIdException(item);
            }

            int targetShelfId = shelfId ?? DefaultShelfId;

            await _itemRepository.AddToShelfAsync(item, targetShelfId, ct);
            return item;
        }
    }
}