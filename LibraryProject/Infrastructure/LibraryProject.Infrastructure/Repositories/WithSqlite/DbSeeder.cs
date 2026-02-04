using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using LibraryProject.Infrastructure.Persistence.InSqlite.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public static class DbSeeder
    {
        private const int DefaultShelfId = 100;

        // Password: admin12345
        private const string PrecomputedHash = "100000.3nTcX8KTECxxTDyrg1XgGg==.dIXcIDSZ4j2Hkx6E2NyP2Z4Gt2GQpLRcyeQqlhd1oqg=";

        public static async Task SeedAsync(LibraryDbContext db, ItemService service, CancellationToken ct = default)
        {
            await db.Database.EnsureDeletedAsync(ct);
            await db.Database.EnsureCreatedAsync(ct);

            Shelf defaultShelf = new Shelf(DefaultShelfId);
            db.Shelves.Add(defaultShelf);

            if (!await db.Items.AnyAsync(ct))
            {
                CreateItemWithAmountTest(
                        defaultShelf,
                        "Die Verwandlung",
                        ItemType.Book,
                        "Franz Kafka",
                        1975,
                        "Die Verwandlung ist eine im Jahr 1912 entstandene Erzählung von Franz Kafka. Die Geschichte handelt von Gregor Samsa, dessen plötzliche Verwandlung in ein „Ungeziefer die Kommunikation seines sozialen Umfelds mit ihm immer mehr hemmt, bis er von seiner Familie für untragbar gehalten wird und schließlich zugrunde geht. ",
                        5,
                        ct);

                CreateItemWithAmountTest(
                        defaultShelf,
                        "Das Schloss ",
                        ItemType.Book,
                        "Franz Kafka",
                        1926,
                        "Das Schloss“ ist ein berühmter, 1922 entstandener und unvollendeter Roman von\r\nFranz Kafka, der 1926 posthum veröffentlicht wurde. Er handelt von dem Landvermesser K. ",
                        10,
                        ct);
            }

            if (!await db.PolicyEntries.AnyAsync(ct))
            {
                db.PolicyEntries.Add(new PolicyEntry
                {
                    Id = Guid.NewGuid(),
                    UserType = UserType.Admin,
                    ItemType = ItemType.Book,
                    PolicyName = "Admin-Book (Test)",
                    Extensions = 2,
                    LoanFees = 0.25m,
                    LoadPeriodInDays = 14
                });

                db.PolicyEntries.Add(new PolicyEntry
                {
                    Id = Guid.NewGuid(),
                    UserType = UserType.Student,
                    ItemType = ItemType.Book,
                    PolicyName = "Student-Book (Test)",
                    Extensions = 1,
                    LoanFees = 0.50m,
                    LoadPeriodInDays = 21
                });
            }

            if (!await db.Users.AnyAsync(ct) && !await db.Accounts.AnyAsync(ct))
            {
                User adminUser = new User("joe", "jonny", "newjersystr", UserType.Admin);
                Account adminAccount = new Account(adminUser, "admin1", PrecomputedHash, "admin@local");
                adminAccount.ReactivateAccount();

                db.Users.Add(adminUser);
                db.Accounts.Add(adminAccount);

                User studentUser = new User("Test", "Student", "teststrasse 1", UserType.Student);
                Account studentAccount = new Account(studentUser, "testStudent", PrecomputedHash, "student@local");
                studentAccount.ReactivateAccount();

                db.Users.Add(studentUser);
                db.Accounts.Add(studentAccount);

                await db.SaveChangesAsync(ct);
            }
        }

        public static void CreateItemWithAmountTest(
        Shelf defaultShelf,
        string name,
        ItemType itemType,
        string author,
        int year,
        string? description,
        int circulationCount,
        CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            var item = new Item(name, itemType, author, year, description, circulationCount);
            defaultShelf.AddItem(item);
            item.AddCopies(circulationCount);
        }
    }
}
