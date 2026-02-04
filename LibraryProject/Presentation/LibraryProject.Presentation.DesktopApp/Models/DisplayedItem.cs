using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Models
{
    public class DisplayedItem
    {
        public Guid Id { get; }
        public string Title { get; }
        public string Author { get; }
        public string Description { get; }
        public int Year { get; }
        public string Type { get; }
        public int AvailableCopies { get; }

        public ArchiveStatus ArchiveStatus { get; }
        public string ArchiveStatusText => ArchiveStatus switch
        {
            ArchiveStatus.Yes => "Ja",
            ArchiveStatus.Partial => "Teilweise",
            ArchiveStatus.No => "Nein",
            _ => "Ungültig"
        };


        public DisplayedItem(Guid id, string title, string author, string description, int year, string type, int availableCopies, ArchiveStatus archiveStatus)
        {
            Id = id;
            Title = title;
            Author = author;
            Description = description;
            Year = year;
            Type = type;
            AvailableCopies = availableCopies;
            ArchiveStatus = archiveStatus;
        }
    }
}
