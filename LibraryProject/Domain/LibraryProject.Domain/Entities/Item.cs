using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Item
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Author { get; private set; }
        public int Year { get; private set; }
        public string? Description { get; private set; }
        public string? CoverImagePath { get; private set; }

        public int ShelfId { get; internal set; }
        public ItemType ItemType { get; private set; }
        public int CirculationCount { get; set; }

        public List<ItemCopy> Copies { get; private set; } = new();

        protected Item() { }
        public Item(string name, ItemType itemType, string author, int year, string? description = null, int circulationCount = 0)
        {
            Id = Guid.NewGuid();
            Name = name;
            ItemType = itemType;
            Author = author;
            Year = year;
            Description = description;
            CirculationCount = circulationCount;
        }

        public void SetShelf(int shelfId) => ShelfId = shelfId;

        public void SetCoverImagePath(string? relativePath)
        {
            CoverImagePath = string.IsNullOrWhiteSpace(relativePath) ? null : relativePath;
        }

        public void UpdateItemName(string newName)
        { 
            Name = newName;
        }

        public void UpdateAuthor(string author)
        {

            Author = author;
        }

        public void UpdateYear(int year)
        {
            Year = year;
        }

        public void UpdateDescription(string? description)
        {
            Description = description;
        }

        public void AddCopies(int amount)
        {
            if (amount <= 0) return;

            for (int i = 0; i < amount; i++)
                Copies.Add(ItemCopy.CreateFor(this));
        }

    }
}
