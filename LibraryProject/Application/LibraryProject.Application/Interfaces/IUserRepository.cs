using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interfaces
{
    public interface IUserRepository
    {
        User? GetExistingUserById(Guid id);
        User? GetExistingUser(string name, UserType userType);
        void SaveUserToStorage(User user);
        void RemoveUserFromStorage(User user);

    }
}
