using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Infrastructure.Persistence.InMemory;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using LibraryProject.Infrastructure.Repositories.WithRem;
using LibraryProject.Infrastructure.Repositories.WithSqlite;
using LibraryProject.Presentation.Shared.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infrastructure
{
    public static class StrategyRegister
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration config)
        {
            String? provider = config["Storage:Provider"]?.Trim() ?? throw new InvalidOperationException($"Storage:Provider is missing in configuration.");

            // AuthorizationService registration belongs in the presentation layer
            // Infrastructure registers the DbContext, Storage, Repositories 
            // Presentation registers Context, Auth, all app services
            if (string.Equals(provider, "Sqlite",StringComparison.OrdinalIgnoreCase))
            {
                // Conncetion string
                var cs = config["Storage:SqliteConnectionString"] ?? throw new InvalidOperationException("The Sqlite connection string is missing.");

                services.AddDbContext<LibraryDbContext>(options => options.UseSqlite(cs));

                services.AddScoped<IAccountRepository, SqliteAccountRepository>();
                //services.AddScoped<IAuthorizationService, AuthorizationService>();
                services.AddScoped<IBorrowingsRepository, SqliteBorrowingRepository>();
                //services.AddScoped<ICurrentUserContext, CurrentUserContext>();
                services.AddScoped<IItemRepository, SqliteItemRepository>();
                services.AddScoped<IPolicyRepository, SqlitePolicyRepository>();
                services.AddScoped<IUserRepository, SqliteUserRepository>();

                return services;
            }
            if (string.Equals(provider, "Rem",StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton<LibraryStorage>();

                services.AddScoped<IAccountRepository, RemAccountRepository>();
                //services.AddScoped<IAuthorizationService, AuthorizationService>();
                services.AddScoped<IBorrowingsRepository, RemBorrowingRepository>();
                //services.AddScoped<ICurrentUserContext, CurrentUserContext>();
                services.AddScoped<IItemRepository, RemItemRepository>();
                services.AddScoped<IPolicyRepository, RemPolicyRepository>();
                services.AddScoped<IUserRepository, RemUserRepository>();

                return services;

            }
            throw new InvalidOperationException($"Unsupported Storage:Provider {provider}. Allowed values: 'Rem' or 'Sqlite'.");
        }
    }
}
