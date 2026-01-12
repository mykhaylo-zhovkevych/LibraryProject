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

        // Persis items even tought not exposed to EF
        public DbSet<Item> Items => Set<Item>();


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PolicyEntry>(b =>
            {
                b.ToTable("Policies");
                b.HasKey(x => new { x.UserType, x.ItemType });

                // Will automatically created with EF
                b.Property(x => x.PolicyName).HasMaxLength(20);
                //b.Property(x => x.LoanFees);
                //b.Property(x => x.Extensions);
                //b.Property(x => x.LoadPeriodInDays);
            });
             
            modelBuilder.Entity<Shelf>(b =>
            {
                b.HasKey(x => x.ShelfId);

                b.Ignore(x => x.Items);

                // Map private field collection
                b.HasMany<Item>("_items")
                                .WithOne()
                                .HasForeignKey(nameof(Item.ShelfId))
                                .OnDelete(DeleteBehavior.Cascade);

                b.Navigation("_items").UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            modelBuilder.Entity<Item>(b =>
            {
                b.HasKey(x => x.Id);

                // ReservedBy is reference to User
                b.HasOne(x => x.ReservedBy)
                    .WithMany()
                    .HasForeignKey(nameof(Item.ReservedById))
                    .OnDelete(DeleteBehavior.SetNull);
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasKey(x => x.Id);

            });

            modelBuilder.Entity<Account>(b =>
            {
                b.HasKey(x => x.AccountId);

                b.HasIndex(x => x.Name).IsUnique();

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

                b.HasOne(x => x.Item)
                    .WithMany()
                    .HasForeignKey(x => x.ItemId)
                    .OnDelete(DeleteBehavior.Restrict);
            });


        }
    }
}
