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

        //public async Task<User> UpdateUserProfile(Guid id, UserType newType)
        //{
        //    User? interestedUser = await _userRepository.GetExistingUserByIdAsync(id);

        //    if (interestedUser == null)
        //    {
        //        throw new NonexistentUserException();
        //    }
        //    _authorizationService.EnsureAuthenticated();

        //    interestedUser.ChangeUserProfile(newType);
        //    return interestedUser;
        //}

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
    }
}
