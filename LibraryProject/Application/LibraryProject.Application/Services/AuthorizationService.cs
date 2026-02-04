using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly ICurrentUserContext _currentUser;

        public AuthorizationService(ICurrentUserContext currentUser)
        {
            _currentUser = currentUser;
        }

        public void EnsureAuthenticated()
        {
            if (!_currentUser.IsAuthorised)
            {
                throw new SecurityException("Benutzer ist nicht angemeldet.");
            }
        }

        public void EnsureAdmin()
        {
            EnsureAuthenticated();

            if (_currentUser.UserType != UserType.Admin)
            {
                throw new SecurityException("Benutzer hat keine Administratorrechte.");
            }
        }
    }
}
