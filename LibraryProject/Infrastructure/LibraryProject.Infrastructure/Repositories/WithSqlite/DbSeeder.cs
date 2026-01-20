using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace LibraryProject.Infrastructure.Repositories.WithSqlite
{
    public static class DbSeeder
    {
        private const int DefaultShelfId = 100;

        public static async Task SeedAsync(LibraryDbContext db, ItemService service, CancellationToken ct = default)
        {
            //await db.Database.EnsureDeletedAsync(ct);
            //await db.Database.EnsureCreatedAsync(ct);

            // Seed Shelf first (required for Item.ShelfId FK)
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
                        "Die Verwandlung ist eine im Jahr 1912 entstandene Erzählung von Franz Kafka. Die Geschichte handelt von Gregor Samsa, dessen plötzliche Verwandlung in ein „Ungeziefer“ die Kommunikation seines sozialen Umfelds mit ihm immer mehr hemmt, bis er von seiner Familie für untragbar gehalten wird und schließlich zugrunde geht. ",
                        5,
                        ct);
                }

            if (!await db.Users.AnyAsync(ct) && !await db.Accounts.AnyAsync(ct))
            {
                User adminUser = new User("admin", UserType.Admin);
                Account adminAccount = new Account(adminUser, "admin1", "admin12345", "admin@local");
                adminAccount.ReactivateAccount();

                db.Users.Add(adminUser);
                db.Accounts.Add(adminAccount);

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
   
            for (int i = 0; i < circulationCount; i++)
            {
                ct.ThrowIfCancellationRequested();

                Item item = new Item(name, itemType, author, year, description, circulationCount);
                defaultShelf.AddItem(item);
            }
        }
    }
}
