using LibraryProject.Domain.Entities;
using LibraryProject.Infrastructure.Persistence.InSqlite.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure.Persistence.InSqlite
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options): base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Shelf> Shelves => Set<Shelf>();
        public DbSet<Borrowing> Borrowings => Set<Borrowing>();
        public DbSet<PolicyEntry> PolicyEntries => Set<PolicyEntry>();

        public DbSet<Item> Items => Set<Item>();
        public DbSet<ItemCopy> ItemCopies => Set<ItemCopy>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PolicyEntry>(b =>
            {
                b.ToTable("Policies");
                b.HasKey(x => new { x.Id });
                // b.HasKey(x => new { x.UserType, x.ItemType });

                b.Property(x => x.PolicyName).HasMaxLength(20);
                //b.Property(x => x.LoanFees);
                //b.Property(x => x.Extensions);
                //b.Property(x => x.LoadPeriodInDays);

                // Optional: enforce uniqueness 
                //b.HasIndex(e => new { e.UserType, e.ItemType, e.PolicyName })
                // .IsUnique();
            });

            modelBuilder.Entity<Shelf>(b =>
            {
                b.HasKey(x => x.ShelfId);
                // If null will auto generate
                //b.Property(x => x.ShelfId).ValueGeneratedOnAdd();

                b.HasMany(s => s.Items)
                 .WithOne()
                 .HasForeignKey(i => i.ShelfId)
                 .OnDelete(DeleteBehavior.Cascade);

                // Write direct to the _items field
                b.Navigation(s => s.Items)
                 .UsePropertyAccessMode(PropertyAccessMode.Field);
            });


            modelBuilder.Entity<Item>(b =>
            {
                b.HasKey(x => x.Id);

                b.HasMany(i => i.Copies)
                    .WithOne(c => c.Item)
                    .HasForeignKey(c => c.ItemId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ItemCopy>(b =>
            {
                b.HasKey(x => x.Id);

                b.HasOne(x => x.ReservedBy)
                    .WithMany()
                    .HasForeignKey(x => x.ReservedById)
                    .OnDelete(DeleteBehavior.SetNull);
            });


            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);

            });

            modelBuilder.Entity<Account>(b =>
            {
                b.HasKey(x => x.AccountId);

                b.HasIndex(x => x.AccountName).IsUnique();

                b.HasOne<User>()
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

            });
            modelBuilder.Entity<Borrowing>(b =>
            {
                b.HasKey(x => x.BorrowingId);

                b.HasOne(x => x.User)
                    .WithMany()
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);

                b.HasOne(x => x.ItemCopy)
                    .WithMany()
                    .HasForeignKey(x => x.ItemCopyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
