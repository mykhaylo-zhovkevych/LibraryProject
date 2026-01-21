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
        Task SaveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default);
        Task RemoveBorrowingAsync(Borrowing borrowing, CancellationToken ct = default);
        //(Item?, User?) GetPossibleBorrowing(Guid userId, Guid itemId);
        Task UpdateBorrowingAsync(Borrowing borrowing, CancellationToken ct = default);

        Task<List<Borrowing>> GetActiveBorrowingsAsync(Guid userId, CancellationToken ct = default);
        Task<List<Borrowing>> GetInactiveBorrowingsAsync(Guid userId, CancellationToken ct = default);
        Task<List<Borrowing>> GetAllBorrowingsAsync(Guid userId, CancellationToken ct = default);

        Task<Borrowing?> GetActiveBorrowingByCopyAsync(Guid userId, Guid itemCopyId, CancellationToken ct = default);
        Task<int> CountActiveBorrowingsForItemAsync(Guid itemId, CancellationToken ct = default);
    }
}
