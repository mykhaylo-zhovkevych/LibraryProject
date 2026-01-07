using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly InMemoryStorage _storage;

        public UserRepository(InMemoryStorage storage)
        {
            _storage = storage;
        }

        public User GetExistingUser(string name, UserType userType)
        {
            return _storage.Users
                .FirstOrDefault(u => u.Name == name && u.UserType == userType);
        }

        public User? GetExistingUserById(Guid id)
        {
            return _storage.Users
                .FirstOrDefault(u => u.Id == id);
        }

        public void SaveUserToStorage(User user)
        {
            _storage.Users.Add(user);
        }

        public void RemoveUserFromStorage(User user)
        {
            _storage.Users.Remove(user);
        }

        // this method dont belong in here
        //public User UpdateUserProfile(Guid id, UserType newType)
        //{
        //    return _storage.UpdateUserProfile(id, newType);
        //}

    }
}
