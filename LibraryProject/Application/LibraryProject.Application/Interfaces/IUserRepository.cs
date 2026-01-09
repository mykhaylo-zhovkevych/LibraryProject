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
        void SaveUser(User user);
        void RemoveUser(User user);
        User? GetExistingUserById(Guid id);
        User? GetExistingUser(string name, UserType userType);

    }
}
