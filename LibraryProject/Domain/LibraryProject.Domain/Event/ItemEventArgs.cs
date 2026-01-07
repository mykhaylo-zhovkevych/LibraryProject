using LibraryProject.Domain.Entities;
using System;

namespace LibraryProject.Domain.Event
{
    public class ItemEventArgs : EventArgs
    {
        public string Message { get; }
        public Item Item { get; }
        public User? ReservedUser { get; }

        public ItemEventArgs(string message, Item item, User? reservedUser = null)
        {
            Message = message;
            Item = item;
            ReservedUser = reservedUser;
        }
    }
}
