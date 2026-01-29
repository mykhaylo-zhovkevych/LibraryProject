using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Infrastructure;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Factories;
using LibraryProject.Presentation.DesktopApp.Services;
using LibraryProject.Presentation.DesktopApp.ViewModels;
using LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels;
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
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
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

            services.AddTransient<ProfileViewModel>();
            services.AddTransient<ManagementViewModel>();
            services.AddTransient<CatalogViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<BorrowingViewModel>();

            services.AddTransient<ItemsViewModel>();
            services.AddTransient<UsersViewModel>();
            services.AddTransient<PoliciesViewModel>();

            // When a GetRequiredService is a called it will pull new instance based on the ApplicationPageNames. Curried function 
            services.AddSingleton<Func<ApplicationPageNames, PageViewModel>>(x => name => name switch
            {
                ApplicationPageNames.Catalog => x.GetRequiredService<CatalogViewModel>(),
                ApplicationPageNames.Borrowing => x.GetRequiredService<BorrowingViewModel>(),
                ApplicationPageNames.Profile => x.GetRequiredService<ProfileViewModel>(),
                ApplicationPageNames.Management => x.GetRequiredService<ManagementViewModel>(),
                ApplicationPageNames.ManagementItems => x.GetRequiredService<ItemsViewModel>(),
                ApplicationPageNames.ManagementUsers => x.GetRequiredService<UsersViewModel>(),
                ApplicationPageNames.ManagementPolicies => x.GetRequiredService<PoliciesViewModel>(),
                _ => throw new InvalidOperationException("The requested page does not exist."),
            });

            services.AddSingleton<PageFactory>();

            return services;
        }
    }
}
