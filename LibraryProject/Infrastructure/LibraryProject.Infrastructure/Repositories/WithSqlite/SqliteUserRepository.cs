using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public class SqliteUserRepository : IUserRepository
    {
        private readonly LibraryDbContext _db;
        public SqliteUserRepository(LibraryDbContext db) => _db = db;

        public async Task<User?> GetExistingUserAsync(string name, UserType userType, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Name == name && u.UserType == userType, ct);
        }

        public async Task<User?> GetExistingUserByIdAsync(Guid id, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            return await _db.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id, ct);
        }

        public async Task RemoveUserAsync(User user, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            _db.Users.Remove(user);
            await _db.SaveChangesAsync(ct);
        }

        public async Task SaveUserAsync(User user, CancellationToken ct)
        {
            ct.ThrowIfCancellationRequested();
            await _db.Users.AddAsync(user, ct);
            await _db.SaveChangesAsync(ct);
        }
    }
}
