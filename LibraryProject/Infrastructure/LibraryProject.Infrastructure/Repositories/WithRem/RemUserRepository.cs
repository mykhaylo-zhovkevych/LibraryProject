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

        public Task<User?> GetExistingUserAsync(string name, UserType userType, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            User? query = _storage.Users
                .FirstOrDefault(u => u.Name == name && u.UserType == userType);
            return Task.FromResult(query);
        }

        public Task<User?> GetExistingUserByIdAsync(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            User? query = _storage.Users
                .FirstOrDefault(u => u.Id == id);
            return Task.FromResult(query);
        }

        public Task SaveUserAsync(User user, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            _storage.Users.Add(user);
            return Task.CompletedTask;
        }

        public Task RemoveUserAsync(User user, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            _storage.Users.Remove(user);
            return Task.CompletedTask;
        }
    }
}
