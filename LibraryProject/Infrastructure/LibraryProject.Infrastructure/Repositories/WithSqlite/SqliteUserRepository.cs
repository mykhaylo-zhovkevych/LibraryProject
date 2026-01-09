using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteUserRepository : IUserRepository
    {
        public Task<User?> GetExistingUserAsync(string name, UserType userType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetExistingUserByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveUserAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
