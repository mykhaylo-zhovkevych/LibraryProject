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

        public UserService(IUserRepository userRepository, IAuthorizationService authorizationService)
        {
            _userRepository = userRepository;
            _authorizationService = authorizationService;
        }

        public async Task<User> CreateUser(string name, UserType userType, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.");
            }
            _authorizationService.EnsureAdmin();

            User newUser = new User(name, userType, default);
            await _userRepository.SaveUserAsync(newUser);
            return newUser;
        }

        public async Task<User> DeleteExistingUser(Guid id)
        {
            User? interestedUser = await _userRepository.GetExistingUserByIdAsync(id);
            if (interestedUser == null)
            {
                throw new NonexistentUserException();
            }
            _authorizationService.EnsureAdmin();

            await _userRepository.RemoveUserAsync(interestedUser);
            return interestedUser;
        }

        public async Task<User?> ReceiveUserByIdAsync(Guid id, CancellationToken ct)
        {
            _authorizationService.EnsureAuthenticated();
            return await _userRepository.GetExistingUserByIdAsync(id, ct);
        }

        public async Task<List<User>> ReceiveAllUsersAsync(CancellationToken ct = default)
        {
            _authorizationService.EnsureAdmin();
            return await _userRepository.GetAllUsersAsync(ct);
        }

        public async Task ConfirmIdentityAsync(Guid userId, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            _authorizationService.EnsureAdmin();

            User user = await _userRepository.GetExistingUserByIdAsync(userId, ct) ?? throw new NonexistentUserException();

            user.ConfirmIdentity();
            await _userRepository.UpdateUserAsync(user, ct);
        }
    }
}
