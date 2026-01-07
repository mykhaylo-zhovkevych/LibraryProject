using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
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

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public (bool, User) CreateNewUser(string name, UserType userType)
        {
            User newUser = new User(name, userType);

            if (string.IsNullOrEmpty(name))
            {
                return (false, null);
            }

            _userRepository.SaveUserToStorage(newUser);
            return (true, newUser);
        }



    }
}
