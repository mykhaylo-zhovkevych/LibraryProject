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
        Task SaveUserAsync(User user, CancellationToken ct = default);
        Task RemoveUserAsync(User user, CancellationToken ct = default);
        Task UpdateUserAsync(User user, CancellationToken ct = default);
        Task<User?> GetExistingUserByIdAsync(Guid id, CancellationToken ct = default);
        Task<User?> GetExistingUserAsync(string name, UserType userType, CancellationToken ct = default);
        Task<User?> GetUserByIdAsync(Guid id, CancellationToken ct = default);
        Task<List<User>> GetAllUsersAsync(CancellationToken ct = default);
    }
}
