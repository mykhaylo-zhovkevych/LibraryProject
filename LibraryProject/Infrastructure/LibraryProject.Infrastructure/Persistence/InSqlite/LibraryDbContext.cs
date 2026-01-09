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
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Shelf> Shelves { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<PolicyEntry> PolicyEntries { get; set; }

        // Persis items even tought not exposed to EF
        public DbSet<Item> Items { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Do i need fetch the connection string from configuration?
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<PolicyEntry>(b =>
            {
                b.ToTable("Policies");
                b.HasKey(x => new { x.UserType, x.ItemType });

                // Look up later
                b.Property(x => x.PolicyName).HasMaxLength(20);
                b.Property(x => x.LoanFees).HasColumnType("decimal(18,2)");
            });

            modelBuilder.Entity<Shelf>(b =>
            {
                b.HasKey(x => x.ShelfId);

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
        }
    }
}
