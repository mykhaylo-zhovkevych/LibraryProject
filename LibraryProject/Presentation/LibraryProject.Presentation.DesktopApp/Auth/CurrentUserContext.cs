using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Auth
{
    public sealed class CurrentUserContext : ICurrentUserContext
    {
        public Guid? UserId { get; private set; }
        public UserType? UserType { get; private set; }
        public bool IsAuthorised => UserId != null;

        public void SignIn(Guid userId, UserType userType)
        {
            UserId = userId;
            UserType = userType;
        }

        public void SignOut()
        {
            UserId = null;
            UserType = null;
        }
    }
}
