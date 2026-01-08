using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
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

        public User CreateUser(string name, UserType userType)
        {
            _authorizationService.EnsureAdmin();
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name is required.");
            }

            User newUser = new User(name, userType);
            _userRepository.SaveUserToStorage(newUser);
             return newUser;
        }

        public User UpdateUserProfile(Guid id, UserType newType)
        {
            _authorizationService.EnsureAuthenticated();
            User? interestedUser = _userRepository.GetExistingUserById(id);

            if (interestedUser == null)
            {
                throw new NonExistingUserException();
            }

            interestedUser.ChangeUserProfile(newType);
            return interestedUser;
        }

        public User DeleteExistingUser(Guid id)
        {
            _authorizationService.EnsureAdmin();
            User? interestedUser = _userRepository.GetExistingUserById(id);
            if (interestedUser == null)
            {
                throw new NonExistingUserException();
            }

            _userRepository.RemoveUserFromStorage(interestedUser);
            return interestedUser;
        }


    }
}
