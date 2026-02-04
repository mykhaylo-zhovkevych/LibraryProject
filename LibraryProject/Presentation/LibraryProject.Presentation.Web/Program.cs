using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Enum;
using LibraryProject.Domain.Exceptions;
using LibraryProject.Infrastructure;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using LibraryProject.Infrastructure.Repositories.WithSqlite;
using LibraryProject.Presentation.Web.Auth;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security;
using App = LibraryProject.Presentation.Web.Components.App;

namespace LibraryProject.Presentation.Web
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            const string FlashError = "flash_error";
            const string FlashInfo = "flash_info";

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCoreServices(builder.Configuration);
            builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICurrentUserContext, CurrentUserContext>();
            builder.Services.AddScoped<AuthOrchestrator>();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/error";
                });

            builder.Services.AddAuthorization();
            builder.Services.AddAuthorizationCore();

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();

            builder.Services.AddAntiforgery();

            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();

            WebApplication app = builder.Build();

            using (IServiceScope scope = app.Services.CreateScope())
            {
                LibraryDbContext db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                ItemService itemService = scope.ServiceProvider.GetRequiredService<ItemService>();
                await DbSeeder.SeedAsync(db, itemService);
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseAntiforgery();

            app.MapPost("/auth/login", async (
                HttpContext http,
                IAntiforgery antiforgery,
                AuthOrchestrator auth,
                [FromForm] string accountName,
                [FromForm] string password,
                [FromForm] UserType selectedType,
                [FromForm] string? returnUrl
            ) =>
            {
                try
                {
                    await antiforgery.ValidateRequestAsync(http);
                    await auth.SignInAsync(http, accountName, password, selectedType);

                    string target = (returnUrl?.StartsWith("/") == true) ? returnUrl : "/";
                    return Results.Redirect(target);
                }
                catch (AntiforgeryValidationException)
                {
                    SetFlash(http, FlashError, "Ungültiger Sicherheits-Token. Bitte Seite neu laden.");
                    return Results.Redirect("/login");
                }
                catch (Exception)
                {
                    SetFlash(http, FlashError, "Login fehlgeschlagen.");
                    return Results.Redirect("/login");
                }
            }).DisableAntiforgery();

            app.MapPost("/auth/logout", async (
                HttpContext http,
                IAntiforgery antiforgery,
                AuthOrchestrator auth
            ) =>
            {
                try
                {
                    await antiforgery.ValidateRequestAsync(http);
                }
                catch (AntiforgeryValidationException)
                {
                }
                await auth.SignOutAsync(http);
                return Results.Redirect("/login");
            }).DisableAntiforgery();

            app.MapPost("/auth/register", async (
                HttpContext http,
                IAntiforgery antiforgery,
                AccountService accountService,
                [FromForm] string accountName,
                [FromForm] string password,
                [FromForm] string email,
                [FromForm] string name,
                [FromForm] string? surname,
                [FromForm] string address,
                [FromForm] UserType customerType
            ) =>
            {
                try
                {
                    await antiforgery.ValidateRequestAsync(http);
                    await accountService.RegisterAccountAsync(accountName, password, email, name, surname, address,customerType, CancellationToken.None);

                    SetFlash(http, FlashInfo, "Registrierung erfolgreich. Bitte einloggen.");
                    return Results.Redirect("/login");
                }
                catch (AntiforgeryValidationException)
                {
                    SetFlash(http, FlashError, "Ungültiger Sicherheits-Token. Bitte Seite neu laden.");
                    return Results.Redirect("/register");
                }
                catch (AccountUsedException ex)
                {
                    SetFlash(http, FlashError, ex.Message);
                    return Results.Redirect("/register");
                }
                catch (Exception ex)
                {
                    SetFlash(http, FlashError, ex.Message);
                    return Results.Redirect("/register");
                }
            }).DisableAntiforgery();

            app.MapGet("/flash", (HttpContext http) =>
            {
                string? error = http.Request.Cookies.TryGetValue(FlashError, out string? e) ? e : null;
                string? info = http.Request.Cookies.TryGetValue(FlashInfo, out string? i) ? i : null;

                if (error != null) http.Response.Cookies.Delete(FlashError);
                if (info != null) http.Response.Cookies.Delete(FlashInfo);

                return Results.Ok(new
                {
                    error = error is null ? null : Uri.UnescapeDataString(error),
                    info = info is null ? null : Uri.UnescapeDataString(info)
                });
            }).AllowAnonymous();

            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();

            static void SetFlash(HttpContext http, string key, string message)
            {
                http.Response.Cookies.Append(
                    key,
                    Uri.EscapeDataString(message),
                    new CookieOptions
                    {
                        HttpOnly = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.Lax,
                        MaxAge = TimeSpan.FromMinutes(1)
                    });
            }
        }
    }
}
