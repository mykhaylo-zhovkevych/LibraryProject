using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
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

        public BorrowingService(IBorrowingsRepository borrowedfRepository, IPolicyRepository policyRepository)
        {
            _borrowedRepository = borrowedfRepository;
            _policyRepository = policyRepository;
        }

        public bool CreateBorrowedItem(User user, Item item)
        {
            if(item.CheckBorrowPossible())
            {
                throw new IsAlreadyBorrowedException(item);
            }

            Policy activePolicy = _policyRepository.GetPolicy(user.UserType, item.ItemType);
            uint allowedCredits = activePolicy.Extensions;

            Borrowing newBorrowing = new Borrowing(user, item, activePolicy);

            _borrowedRepository.SaveBorrowingToStorage(newBorrowing);
            item.BorrowItem();

            return true;
        }

        public bool ReturnBorrowedItem(User user, Item item)
        {
            Borrowing activeBorrowing = _borrowedRepository
                .GetActiveBorrowings(user.Id)
                .FirstOrDefault(b => b.Item.Id == item.Id && b.Item.Id == item.Id);

            if (activeBorrowing == null)
            {
                throw new ArgumentException($"No entries was found for this user {user.Name}");
            }

            activeBorrowing.ReturnBorrowing(activeBorrowing);

            if (activeBorrowing.Item.IsReserved)
            {
                OnInformReserver(new ItemEventArgs($"The {activeBorrowing.Item.Name} is now available", activeBorrowing.Item, item.ReservedBy));
            }

            return true;
        }

        public bool ExtendBorrowingPeriod(User user, Item item)
        {
            Borrowing activeBorrowing = _borrowedRepository
                .GetActiveBorrowings(user.Id)
                .FirstOrDefault(b => b.Item.Id == item.Id && 
                                b.Item.Id == item.Id);

            if (activeBorrowing == null)
            {
                throw new ArgumentException($"No entries was found for this user {user.Name}");
            }

            return activeBorrowing.Extend();
        }

    }
}
