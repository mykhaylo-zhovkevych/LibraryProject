using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Dto
{
    public sealed class LoginSession
    {
        public Guid UserId { get; }
        public UserType UserType { get; }
        public string UserName { get; }

        public LoginSession(Guid userId, UserType userType, string userName)
        {
            UserId = userId;
            UserType = userType;
            UserName = userName;
        }
    }
}
