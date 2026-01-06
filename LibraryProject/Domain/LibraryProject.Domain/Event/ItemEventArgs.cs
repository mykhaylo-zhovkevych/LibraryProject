using LibraryProject.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Event
{
    public class ItemEventArgs : EventArgs
    {
        public string Message { get; set; }
        public Item Item { get; }
        public User? ReservedUser { get; set; }

        public ItemEventArgs(string message, Item item, User? reservedUser = null)
        {
            Message = message;
            Item = item;
            ReservedUser = reservedUser;
        }
    }
}
