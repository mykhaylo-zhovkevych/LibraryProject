using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Entities
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public UserType UserType { get; private set; }

        protected User() { }
        public User(string name, UserType userType)
        {
            Id = Guid.NewGuid();
            Name = name;
            UserType = userType;
        }
    }
}
