using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class BorrowingViewModel : PageViewModel
    {
        private readonly ICurrentUserContext _currentUserContext;
        private readonly BorrowingService _borrowingService;
        private readonly UserService _userService;

        public ObservableCollection<DisplayedBorrowing> Borrowings { get; } = new();
        public BorrowingViewModel(ICurrentUserContext currentUserContext, BorrowingService borrowingService, UserService userService)
        {
            _currentUserContext = currentUserContext;
            _borrowingService = borrowingService;
            _userService = userService;
            PageName = ApplicationPageNames.Borrowing;
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync(CancellationToken ct = default)
        {
            var userId = _currentUserContext.UserId.Value;
            var borrowings = await _borrowingService.SearchAllBorrowingsByUserId(userId, ct);

            Borrowings.Clear();
            foreach (var b in borrowings)
            {
                ct.ThrowIfCancellationRequested();
                Borrowings.Add(MapBorrowingToDisplayedBorrowing(b));
            }
        }


        [RelayCommand]
        private async Task Expand(DisplayedBorrowing db)
        {
            try
            {
                var ct = CancellationToken.None;

                Guid userId = _currentUserContext.UserId.Value;
                User currentUser = await _userService.GetUserByIdAsync(userId, ct);

                Item currentBorrowingItem = (await _borrowingService.SearchForActiveBorrowingsByUserId(userId, ct))
                    .FirstOrDefault(b => b.Item.Name == db.BorrowingItemName && b.LoanDate == db.LoanDate).Item;

                await _borrowingService.ExtendBorrowingPeriodAsync(currentUser, currentBorrowingItem, ct);
                await LoadDataAsync(ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while extending the borrowing period: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task Return(DisplayedBorrowing db)
        {
            try
            {
                var ct = CancellationToken.None;

                Guid userId = _currentUserContext.UserId.Value;
                User currentUser = await _userService.GetUserByIdAsync(userId, ct) ?? throw new InvalidOperationException("User not found.");

                Item currentBorrowingItem = (await _borrowingService.SearchForActiveBorrowingsByUserId(userId, ct))
                    .FirstOrDefault(b => b.Item.Name == db.BorrowingItemName && b.LoanDate == db.LoanDate).Item;

                await _borrowingService.ReturnBorrowedItemAsync(currentUser, currentBorrowingItem, ct);
                await LoadDataAsync(ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while returning the borrowing: {ex.Message}");
            }
        }


        private DisplayedBorrowing MapBorrowingToDisplayedBorrowing(Borrowing borrowing)
        {
            return new DisplayedBorrowing(
                    borrowing.Item.Name,
                    borrowing.Item.Author,
                    borrowing.Item.ItemType.ToString(),
                    borrowing.LoanDate,
                    borrowing.DueDate,
                    borrowing.IsReturned ? "Returned" : "On Loan"
                    );

        }
    }
}
