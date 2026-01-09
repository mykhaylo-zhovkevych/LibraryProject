using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Event;
using LibraryProject.Domain.Exceptions;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services
{
    public class BorrowingService
    {
        private readonly IBorrowingsRepository _borrowedRepository;
        private readonly IPolicyRepository _policyRepository;
        private readonly IAuthorizationService _authorizationService;

        public event EventHandler<ItemEventArgs>? InformReserver;

        public BorrowingService(IBorrowingsRepository borrowedfRepository, IPolicyRepository policyRepository, IAuthorizationService authorizationService)
        {
            _borrowedRepository = borrowedfRepository;
            _policyRepository = policyRepository;
            _authorizationService = authorizationService;
        }

        private void OnInformReserver(ItemEventArgs e)
        {
            InformReserver?.Invoke(this, e);
        }

        public async Task<bool> CreateBorrowedItem(User user, Item item, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();

            if (!item.CheckBorrowPossible())
            {
                throw new IsAlreadyBorrowedException(item);
            }

            Policy activePolicy = await _policyRepository.GetPolicyAsync(user.UserType, item.ItemType, ct) ?? throw new NonexistentPolicyException();
            // uint allowedCredits = activePolicy.Extensions;

            Borrowing newBorrowing = new Borrowing(user, item, activePolicy);

            await _borrowedRepository.SaveBorrowingAsync(newBorrowing);
            item.BorrowItem();
            return true;
        }

        public async Task<bool> ReturnBorrowedItem(User user, Item item, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            Borrowing? activeBorrowing = await _borrowedRepository.GetActiveBorrowingAsync(user.Id, item.Id, ct);

            if (activeBorrowing == null)
            {
                throw new ArgumentException($"No entries was found for this user {user.Name}");
            }

            activeBorrowing.Item.ReturnItem();
            activeBorrowing.ReturnBorrowing(activeBorrowing);

            if (activeBorrowing.Item.IsReserved)
            {
                OnInformReserver(new ItemEventArgs($"The {activeBorrowing.Item.Name} is now available", activeBorrowing.Item, item.ReservedBy));
            }

            return true;
        }

        public async Task<bool> ExtendBorrowingPeriod(User user, Item item, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            Borrowing? activeBorrowing = await _borrowedRepository.GetActiveBorrowingAsync(user.Id, item.Id, ct);

            if (activeBorrowing == null)
            {
                throw new ArgumentException($"No entries was found for this user {user.Name}");
            }

            return activeBorrowing.Extend();
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
