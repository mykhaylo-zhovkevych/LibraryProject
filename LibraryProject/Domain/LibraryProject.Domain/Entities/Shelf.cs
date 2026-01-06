using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class Shelf
    {
        public int ShelfId { get; private set; }
        
        public Shelf(int shelfNumber)
        {
            ShelfId = shelfNumber;
        }

    }
}
