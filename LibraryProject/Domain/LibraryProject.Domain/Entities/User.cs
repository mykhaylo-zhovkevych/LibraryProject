using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string? Surname { get; private set; }
        public string Address { get; private set; }
        public UserType UserType { get; private set; }
        //public bool IsAuthentication { get; private set; } = false;

        protected User() { }
        public User(string name, string? surname, string address, UserType userType)
        {
            Id = Guid.NewGuid();
            Name = name;
            Surname = surname;
            Address = address;
            UserType = userType;
            //IsAuthentication = isAuthenticated;
        }

        //public void ConfirmIdentity()
        //{
        //    IsAuthentication = true;
        //}
    }
}
