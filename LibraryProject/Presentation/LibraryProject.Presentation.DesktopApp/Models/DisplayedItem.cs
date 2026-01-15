using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Models
{
    public class DisplayedItem
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
        public int AvailableCopies { get; set; }
        public int TotalCopies { get; set; }

        public DisplayedItem(string title, string author, string description, int year, string type, int availableCopies, int totalCopies)
        {
            Title = title;
            Author = author;
            Description = description;
            Year = year;
            Type = type;
            AvailableCopies = availableCopies;
            TotalCopies = totalCopies;
        }

    }
}
