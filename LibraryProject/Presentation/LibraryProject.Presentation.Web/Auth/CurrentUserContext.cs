using LibraryProject.Application.Interfaces;
using LibraryProject.Domain.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Globalization;
using System.Security.Claims;

namespace LibraryProject.Presentation.Web.Auth
{
    public sealed class CurrentUserContext : ICurrentUserContext
    {
        private readonly IHttpContextAccessor _httpAccessor;
        public CurrentUserContext(IHttpContextAccessor accessor) => _httpAccessor = accessor;

        private ClaimsPrincipal? User => _httpAccessor.HttpContext?.User;

        public bool IsAuthorised => User?.Identity?.IsAuthenticated ?? false;

        public Guid? UserId
        {
            get
            {
                string? raw = User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return Guid.TryParse(raw, out Guid id) ? id : null;
            }
        }

        public UserType? UserType
        {
            get
            {
                string? raw = User?.FindFirstValue(ClaimTypes.Role);
                return Enum.TryParse<UserType>(raw, ignoreCase: true, out UserType t) ? t : null;
            }
        }

        public void SignIn(Guid userId, UserType userType) => throw new NotImplementedException();
        public void SignOut() => throw new NotImplementedException();
    }
}