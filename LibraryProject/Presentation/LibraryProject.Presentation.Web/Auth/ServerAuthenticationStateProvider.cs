using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace LibraryProject.Presentation.Web.Auth
{
    public class ServerAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServerAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            HttpContext? httpContext = _httpContextAccessor.HttpContext;
            ClaimsPrincipal user = httpContext?.User ?? new ClaimsPrincipal(new ClaimsIdentity());

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}