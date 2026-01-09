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

            User newUser = new User(name, userType);
            await _userRepository.SaveUserAsync(newUser);
            return newUser;
        }

        public async Task<User> UpdateUserProfile(Guid id, UserType newType)
        {
            _authorizationService.EnsureAuthenticated();
            User? interestedUser = await _userRepository.GetExistingUserByIdAsync(id);

            if (interestedUser == null)
            {
                throw new NonexistentUserException();
            }

            interestedUser.ChangeUserProfile(newType);
            return interestedUser;
        }

        public async Task<User> DeleteExistingUser(Guid id)
        {
            _authorizationService.EnsureAdmin();
            User? interestedUser = await _userRepository.GetExistingUserByIdAsync(id);
            if (interestedUser == null)
            {
                throw new NonexistentUserException();
            }

            await _userRepository.RemoveUserAsync(interestedUser);
            return interestedUser;
        }
    }
}
