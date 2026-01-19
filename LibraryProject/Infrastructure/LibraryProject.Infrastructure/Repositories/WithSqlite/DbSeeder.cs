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

        public static async Task SeedAsync(LibraryDbContext db, CancellationToken ct = default)
        {
            //await db.Database.EnsureDeletedAsync(ct);
            //await db.Database.EnsureCreatedAsync(ct);

            // Seed Shelf first (required for Item.ShelfId FK)
            Shelf defaultShelf = new Shelf(DefaultShelfId);
            db.Shelves.Add(defaultShelf);
           

            if (!await db.Items.AnyAsync(ct))
            {
                Item item1 = new Item(
                    "Die Verwandlung",
                    ItemType.Book,
                    "Franz Kafka",
                    1975,
                    "Die Verwandlung ist eine im Jahr 1912 entstandene Erzählung von Franz Kafka. Die Geschichte handelt von Gregor Samsa, dessen plötzliche Verwandlung in ein „Ungeziefer“ die Kommunikation seines sozialen Umfelds mit ihm immer mehr hemmt, bis er von seiner Familie für untragbar gehalten wird und schließlich zugrunde geht. ",
                    10);

                Item item2 = new Item(
                    "Der Prozess",
                    ItemType.Book,
                    "Franz Kafka",
                    1925,
                    "Der Prozess ist ein Roman von Franz Kafka, der posthum im Jahr 1925 veröffentlicht wurde. Die Geschichte handelt von Josef K., einem Bankangestellten, der eines Morgens ohne ersichtlichen Grund verhaftet wird und sich in einem undurchsichtigen Justizsystem wiederfindet. Trotz seiner Bemühungen, den Grund für seine Verhaftung zu erfahren und sich zu verteidigen, gerät Josef K. immer tiefer in einen Albtraum aus Bürokratie, Schuld und Verzweiflung.",
                    5);

                Item item3 = new Item(
                    "Die Blechtrommel",
                    ItemType.Book,
                    "Günter Grass",
                    1959,
                    "Die Blechtrommel ist ein Roman von Günter Grass, der 1959 veröffentlicht wurde. Die Geschichte wird aus der Perspektive von Oskar Matzerath erzählt, einem Jungen, der beschließt, im Alter von drei Jahren nicht mehr zu wachsen und stattdessen die Welt mit seiner Blechtrommel zu kommentieren. Der Roman behandelt Themen wie Krieg, Schuld und die deutsche Geschichte des 20. Jahrhunderts.",
                    4);

                defaultShelf.AddItem(item1);
                defaultShelf.AddItem(item2);
                defaultShelf.AddItem(item3);

         
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
    }
}
