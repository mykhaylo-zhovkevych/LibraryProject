using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Models
{
    public class DisplayedUser
    {
        public Guid Id { get; }
        public string Name { get; }
        public string UserType { get; }

        public DisplayedUser(Guid id, string name, string userType)
        {
            Id = id;
            Name = name;
            UserType = userType;
        }
    }
}
