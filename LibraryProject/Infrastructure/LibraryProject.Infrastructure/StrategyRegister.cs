using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Infrastructure.Persistence.InMemory;
using LibraryProject.Infrastructure.Repositories.WithRem;
using LibraryProject.Presentation.Shared.Auth;
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
            String? provider = config["Storage:Provider"]?.Trim();

            if (provider == null)
            {
                throw new InvalidOperationException($"The specified config provider is not missing");
            }

            if (string.Equals(provider, "Sqlite",StringComparison.OrdinalIgnoreCase))
            {
                // Conncetion string



            }

            else if (string.Equals(provider, "Rem",StringComparison.OrdinalIgnoreCase))
            {
                services.AddSingleton<LibraryStorage>();

                services.AddScoped<IAccountRepository, RemAccountRepository>();
                services.AddScoped<IAuthorizationService, AuthorizationService>();
                services.AddScoped<IBorrowingsRepository, RemBorrowingRepository>();
                services.AddScoped<ICurrentUserContext, CurrentUserContext>();
                services.AddScoped<IItemRepository, RemItemRepository>();
                services.AddScoped<IPolicyRepository, RemPolicyRepository>();
                services.AddScoped<IUserRepository, RemUserRepository>();

            }
            else
            {
                throw new InvalidOperationException($"The specified config {provider} is not supported");
            }
            return services;
        }
    }
}
