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

        public bool CreateBorrowedItem(User user, Item item)
        {
            _authorizationService.EnsureAuthenticated();

            if (!item.CheckBorrowPossible())
            {
                throw new IsAlreadyBorrowedException(item);
            }

            Policy activePolicy = _policyRepository.GetPolicy(user.UserType, item.ItemType) ?? throw new NonExistingPolicyException();
            // uint allowedCredits = activePolicy.Extensions;

            Borrowing newBorrowing = new Borrowing(user, item, activePolicy);

            _borrowedRepository.SaveBorrowing(newBorrowing);
            item.BorrowItem();

            return true;
        }

        public bool ReturnBorrowedItem(User user, Item item)
        {
            _authorizationService.EnsureAuthenticated();

            Borrowing? activeBorrowing = _borrowedRepository
                .GetActiveBorrowings(user.Id)
                .FirstOrDefault(b => b.Item.Id == item.Id);

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

        public bool ExtendBorrowingPeriod(User user, Item item)
        {
            _authorizationService.EnsureAuthenticated();

            Borrowing? activeBorrowing = _borrowedRepository
                .GetActiveBorrowings(user.Id)
                .FirstOrDefault(b => b.Item.Id == item.Id && 
                                b.Item.Id == item.Id);

            if (activeBorrowing == null)
            {
                throw new ArgumentException($"No entries was found for this user {user.Name}");
            }

            return activeBorrowing.Extend();
        }

        public List<Borrowing> SearchAllBorrowingsByUserId(Guid userId)
        {
            _authorizationService.EnsureAdmin();
            List<Borrowing> borrowings = _borrowedRepository.GetAllBorrowings(userId);
            return borrowings;
        }

        public List<Borrowing> SearchForActiveBorrowingsByUserId(Guid userId)
        {
            _authorizationService.EnsureAuthenticated();

            List<Borrowing> borrowings = _borrowedRepository.GetActiveBorrowings(userId);
            return borrowings;
        }

        public List<Borrowing> SearchForInactiveBorrowingsByUserId(Guid userId)
        {
            _authorizationService.EnsureAuthenticated();

            List<Borrowing> borrowings = _borrowedRepository.GetInactiveBorrowings(userId);
            return borrowings;
        }
    }
}
