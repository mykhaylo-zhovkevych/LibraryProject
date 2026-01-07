using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interfaces
{
    public interface IBorrowingsRepository
    {
        void SaveBorrowingToStorage(Borrowing borrowing);
        void RemoveBorrowingFromStorage(Borrowing borrowing);
        (Item?, User?) GetPossibleBorrowing(Guid userId, Guid itemId);
        List<Borrowing> GetActiveBorrowings(Guid userId);


    }
}
