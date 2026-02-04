using LibraryProject.Application.Dto;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Enum;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.ComponentModel;
using System.Security.Claims;

namespace LibraryProject.Presentation.Web.Auth
{
    public sealed class AuthOrchestrator
    {
        private readonly AccountService _accountService;

        public AuthOrchestrator(AccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task SignInAsync(HttpContext http, string accountName, string password, UserType selectedType)
        {
            LoginSession session = await _accountService.LoginAsync(accountName, password, selectedType, CancellationToken.None);

            List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, session.UserId.ToString()),
                    new Claim(ClaimTypes.Name, session.UserName),
                    new Claim(ClaimTypes.Role, session.UserType.ToString()),
                };

            ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            await http.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });

        }

        public Task SignOutAsync(HttpContext http) => http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    }
}