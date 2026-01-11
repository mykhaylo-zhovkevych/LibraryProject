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
        public Guid Id { get; set; }
        public string Name { get; set; }
        public UserType UserType { get; set; }

        protected User() { }
        public User(string name, UserType userType)
        {
            Id = Guid.NewGuid();
            Name = name;
            UserType = userType;
        }

        public void ChangeUserProfile(UserType selectedUserType)
        {
            UserType = selectedUserType;
        }
    }
}
