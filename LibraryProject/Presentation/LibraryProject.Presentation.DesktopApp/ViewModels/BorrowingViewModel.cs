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
using System.Runtime.InteropServices;
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
        private bool _initialized;

        public ObservableCollection<DisplayedBorrowing> Borrowings { get; } = new();
        public BorrowingViewModel(ICurrentUserContext currentUserContext, BorrowingService borrowingService, UserService userService)
        {
            _currentUserContext = currentUserContext;
            _borrowingService = borrowingService;
            _userService = userService;
            PageName = ApplicationPageNames.Borrowing;
        }

        public string BorrowingsSubtitle => $"Sie haben insgesamt {TotalActiveBorrowings} aktive Ausleihe(n).";
        private int TotalActiveBorrowings => Borrowings.Count(b => b.Status == "Ausgeliehen");

        public async Task InitializedAsync()
        {
            if (_initialized) return;
            _initialized = true;

            await LoadDataAsync();
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

            OnPropertyChanged(nameof(TotalActiveBorrowings));
            OnPropertyChanged(nameof(BorrowingsSubtitle));
        }


        [RelayCommand]
        private async Task Expand(DisplayedBorrowing db)
        {
            try
            {
                CancellationToken ct = CancellationToken.None;
                Guid userId = _currentUserContext.UserId.Value;

                Borrowing borrowing = (await _borrowingService.SearchForActiveBorrowingsByUserId(userId, ct)).First(b => b.BorrowingId == db.BorrowingId);
                User currentUser = await _userService.ReceiveUserByIdAsync(userId, ct) ?? throw new InvalidOperationException("User not found");

                await _borrowingService.ExtendBorrowingPeriodAsync(
                    currentUser,
                    borrowing.ItemCopy.Id,
                    ct);

                await LoadDataAsync(ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Extend error: {ex.Message}");
            }
        }


        [RelayCommand]
        private async Task Return(DisplayedBorrowing db)
        {
            try
            {
                CancellationToken ct = CancellationToken.None;
                Guid userId = _currentUserContext.UserId.Value;

                Borrowing borrowing = (await _borrowingService.SearchForActiveBorrowingsByUserId(userId, ct)).First(b => b.BorrowingId == db.BorrowingId);
                User currentUser = await _userService.ReceiveUserByIdAsync(userId, ct) ?? throw new InvalidOperationException("User not found");

                await _borrowingService.ReturnBorrowedItemAsync(
                    currentUser,
                    borrowing.ItemCopy.Id,
                    ct);

                Borrowings.Remove(db);

                await LoadDataAsync(ct);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while returning the borrowing: {ex.Message}");
            }
        }


        private DisplayedBorrowing MapBorrowingToDisplayedBorrowing(Borrowing b)
        {
            Item item = b.ItemCopy.Item;
            return new DisplayedBorrowing(
                b.BorrowingId,
                b.ItemCopyId,
                item.Name,
                item.Author,
                item.ItemType.ToString(),
                b.LoanDate,
                b.DueDate,
                b.IsReturned ? "Retourniert" : "Ausgeliehen"
            );

        }
    }
}
