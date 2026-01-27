using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Event;
using LibraryProject.Domain.Exceptions;
using LibraryProject.Domain.Exceptions.Nonexistent;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace LibraryProject.Application.Services
{
    public class BorrowingService
    {
        private readonly IBorrowingsRepository _borrowedRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IItemRepository _itemRepository;

        public event EventHandler<ItemEventArgs>? InformReserver;

        public BorrowingService(IBorrowingsRepository borrowedfRepository, IPolicyRepository policyRepository, IAuthorizationService authorizationService, IItemRepository itemRepository)
        {
            _borrowedRepository = borrowedfRepository;
            _policyRepository = policyRepository;
            _authorizationService = authorizationService;
            _itemRepository = itemRepository;
        }

        private void OnInformReserver(ItemEventArgs e)
        {
            InformReserver?.Invoke(this, e);
        }

        public async Task CreateBorrowedItemAsync(User user, Item item, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            Policy activePolicy = await _policyRepository.GetPolicyAsync(user.UserType, item.ItemType, ct) ?? throw new NonexistentPolicyException();
            ItemCopy copy = await _itemRepository.GetCopyToBorrowAsync(item.Id, user.Id, ct) ?? throw new ArgumentException("No available copy.");

            if (!copy.CheckBorrowPossible(user.Id))
            {
                throw new IsAlreadyBorrowedException(item);
            }
            if (copy.ReservedById == user.Id)
            {
                copy.ReturnItem();
            }

            copy.BorrowItem();

            await _itemRepository.UpdateCirculationCountAsync(item.Id, -1, ct);
            await _itemRepository.UpdateCopyAsync(copy, ct);

            Borrowing borrowing = new Borrowing(user, copy, activePolicy);
            await _borrowedRepository.SaveBorrowingAsync(borrowing, ct);
        }

        public async Task ReturnBorrowedItemAsync(User user, Guid itemCopyId, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            Borrowing activeBorrowing = await _borrowedRepository.GetActiveBorrowingByCopyAsync(user.Id, itemCopyId, ct) ?? throw new ArgumentException($"No active entry was found for: {user.Name}");

            activeBorrowing.ReturnBorrowing();

            await _itemRepository.UpdateCirculationCountAsync(activeBorrowing.ItemCopy.ItemId, +1, ct);
            await _itemRepository.UpdateCopyAsync(activeBorrowing.ItemCopy, ct);
            await _borrowedRepository.UpdateBorrowingAsync(activeBorrowing, ct);

            if (activeBorrowing.ItemCopy.IsReserved && activeBorrowing.ItemCopy.ReservedById != null)
            {
                OnInformReserver(new ItemEventArgs(
                    $"The {activeBorrowing.ItemCopy.Item.Name} is now available",
                    activeBorrowing.ItemCopy.Item,
                    activeBorrowing.ItemCopy.ReservedBy
                ));
            }
        }


        public async Task ExtendBorrowingPeriodAsync(User user, Guid itemCopyId, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            Borrowing activeBorrowing = await _borrowedRepository.GetActiveBorrowingByCopyAsync(user.Id, itemCopyId, ct) ?? throw new ArgumentException($"No active entry was found for: {user.Name}");

            activeBorrowing.Extend();
            await _borrowedRepository.UpdateBorrowingAsync(activeBorrowing, ct);
        }


        public async Task<List<Borrowing>> SearchAllBorrowingsByUserId(Guid userId, CancellationToken ct)
        {
            _authorizationService.EnsureAdmin();
            List<Borrowing> borrowings =  await _borrowedRepository.GetAllBorrowingsAsync(userId, ct);
            return borrowings;
        }


        public async Task<List<Borrowing>> SearchForActiveBorrowingsByUserId(Guid userId, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            List<Borrowing> borrowings = await _borrowedRepository.GetActiveBorrowingsAsync(userId, ct);
            return borrowings;
        }

        public async Task<List<Borrowing>> SearchForInactiveBorrowingsByUserId(Guid userId, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            List<Borrowing> borrowings = await _borrowedRepository.GetInactiveBorrowingsAsync(userId, ct);
            return borrowings;
        }
    }
}
