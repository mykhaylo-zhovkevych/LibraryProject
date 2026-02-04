using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
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
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddSingleton<IConfiguration>(config);

            // Infrastructure (DB + Repositories)
            services.AddInfrastructureServices(config);

            services.AddScoped<ICurrentUserContext, CurrentUserContext>();

            // Application Services
            services.AddScoped<IAuthorizationService, AuthorizationService>();

            services.AddScoped<AccountService>();
            services.AddScoped<UserService>();
            services.AddScoped<ItemService>();
            services.AddScoped<BorrowingService>();
            services.AddScoped<PolicyService>();

            return services;
        }
    }
}
