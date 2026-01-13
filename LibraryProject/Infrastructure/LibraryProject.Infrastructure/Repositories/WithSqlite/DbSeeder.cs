using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(LibraryDbContext db, CancellationToken ct = default)
        {
            await db.Database.EnsureCreatedAsync(ct);

            if(await db.Users.AnyAsync(ct))
            {
                return;
            }
            if (await db.Accounts.AnyAsync(ct))
            {
                return;
            }
            User adminUser = new User("admin", UserType.Admin);

            Account adminAccount = new Account(adminUser, "admin1", "admin12345", "admin@local");

            adminAccount.ReactivateAccount();

            db.Users.Add(adminUser);
            db.Accounts.Add(adminAccount);

            await db.SaveChangesAsync(ct);
        }
    }
}
