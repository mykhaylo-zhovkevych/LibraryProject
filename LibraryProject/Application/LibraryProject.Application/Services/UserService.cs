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
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthorizationService _authorizationService;
        private readonly IBorrowingsRepository _borrowingsRepository;
        private readonly IItemRepository _itemRepository;

        public UserService(IUserRepository userRepository, IAuthorizationService authorizationService, IBorrowingsRepository borrowingsRepository, IItemRepository itemRepository)
        {
            _userRepository = userRepository;
            _authorizationService = authorizationService;
            _borrowingsRepository = borrowingsRepository;
            _itemRepository = itemRepository;
        }

        public async Task<User> CreateUser(string name, string surname, string address, UserType userType, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            _authorizationService.EnsureAdmin();

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name ist erforderlich.");
            }
            _authorizationService.EnsureAdmin();

            User newUser = new User(name, surname, address, userType);
            await _userRepository.SaveUserAsync(newUser);
            return newUser;
        }

        public async Task DeleteExistingUserAsync(Guid userId, CancellationToken ct = default)
        {
            if (_authorizationService is null) throw new NullReferenceException("_authorizationService ist null");
            if (_userRepository is null) throw new NullReferenceException("_userRepository ist null");
            if (_borrowingsRepository is null) throw new NullReferenceException("_borrowingsRepository ist null");
            if (_itemRepository is null) throw new NullReferenceException("_itemRepository ist null");

            ct.ThrowIfCancellationRequested();
            _authorizationService.EnsureAdmin();

            User? interestedUser = await _userRepository.GetExistingUserByIdAsync(userId);
            if (interestedUser == null)
            {
                throw new NonexistentUserException();
            }

            if (await _borrowingsRepository.HasAnyBorrowingsAsync(userId, ct))
            {
                throw new InvalidOperationException("Benutzer kann nicht gelöscht werden: Ausleihhistorie existiert.");
            }
            if (await _itemRepository.HasAnyReservationsAsync(userId, ct))
            {
                throw new InvalidOperationException("Benutzer kann nicht gelöscht werden: Es existieren Reservierungen.");
            }

            await _userRepository.RemoveUserAsync(interestedUser);
        }

        public async Task<User?> ReceiveUserByIdAsync(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            _authorizationService.EnsureAuthenticated();
            return await _userRepository.GetExistingUserByIdAsync(id, ct);
        }

        public async Task<List<User>> ReceiveAllUsersAsync(CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _authorizationService.EnsureAdmin();
            return await _userRepository.GetAllUsersAsync(ct);
        }

        //public async Task ConfirmIdentityAsync(Guid userId, CancellationToken ct = default)
        //{
        //    ct.ThrowIfCancellationRequested();
        //    _authorizationService.EnsureAdmin();

        //    User user = await _userRepository.GetExistingUserByIdAsync(userId, ct) ?? throw new NonexistentUserException();

        //    user.ConfirmIdentity();
        //    await _userRepository.UpdateUserAsync(user, ct);
        //}
    }
}
