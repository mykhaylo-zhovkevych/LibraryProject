using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Exceptions
{
    public class ItemUsedByException : Exception
    {
        public ItemUsedByException(Item item) : base($"Apology, but {item.Name} is allready reserved") { }
    }
}
