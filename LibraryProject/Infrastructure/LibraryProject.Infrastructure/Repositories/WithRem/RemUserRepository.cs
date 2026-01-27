using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InMemory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithRem
{
    public class RemUserRepository : IUserRepository
    {
        private readonly LibraryStorage _storage;
        public RemUserRepository(LibraryStorage storage) => _storage = storage;

        public Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<User>> GetAllUsersAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task SaveUserAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserAsync(User user, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetExistingUserByIdAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<User?> GetExistingUserAsync(string name, UserType userType, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
