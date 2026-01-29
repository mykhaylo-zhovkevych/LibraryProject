using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interfaces
{
    public interface ICurrentUserContext
    {
        Guid? UserId { get; }
        UserType? UserType { get; }
        bool IsAuthorised { get; }

        void SignIn(Guid userId, UserType userType);
        void SignOut();
    }
}
