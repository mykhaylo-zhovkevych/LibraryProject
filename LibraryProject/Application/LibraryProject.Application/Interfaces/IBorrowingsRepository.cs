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
        void SaveBorrowing(Borrowing borrowing);
        void RemoveBorrowing(Borrowing borrowing);
        //(Item?, User?) GetPossibleBorrowing(Guid userId, Guid itemId);
        List<Borrowing> GetActiveBorrowings(Guid userId);
        List<Borrowing> GetInactiveBorrowings(Guid userId);
        List<Borrowing> GetAllBorrowings(Guid userId);

    }
}
