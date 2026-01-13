using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Infrastructure;
using LibraryProject.Presentation.DesktopApp.Services;
using LibraryProject.Presentation.DesktopApp.ViewModels;
using LibraryProject.Presentation.Shared.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            // Infrastructure
            services.AddSingleton<IConfiguration>(config);
            services.AddInfrastructureServices(config);

            // Session
            services.AddSingleton<ICurrentUserContext, CurrentUserContext>();

            // Application services
            services.AddScoped<IAuthorizationService, AuthorizationService>();
            services.AddScoped<AccountService>();
            services.AddScoped<UserService>();
            services.AddScoped<ItemService>();
            services.AddScoped<BorrowingService>();
            services.AddScoped<PolicyService>();

            // Navigation
            services.AddScoped<INavigationService, NavigationService>();

            // ViewModels
            services.AddSingleton<MainViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();

            return services;
        }
    }
}
