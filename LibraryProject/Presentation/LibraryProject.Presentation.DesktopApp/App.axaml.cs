using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using LibraryProject.Application.Interfaces;
using LibraryProject.Application.Services;
using LibraryProject.Infrastructure;
using LibraryProject.Infrastructure.Persistence.InSqlite;
using LibraryProject.Infrastructure.Repositories.WithSqlite;
using LibraryProject.Presentation.DesktopApp.Services;
using LibraryProject.Presentation.DesktopApp.ViewModels;
using LibraryProject.Presentation.DesktopApp.Views;
using LibraryProject.Presentation.Shared.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp
{
    public partial class App : Avalonia.Application
    {
        private ServiceProvider? _rootProvider;
        private IServiceScope? _appScope;

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);
                DisableAvaloniaDataAnnotationValidation();

                // Load config file
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var services = new ServiceCollection();

                // DbContext/Storage + Repos
                services.AddSingleton<IConfiguration>(config);
                services.AddInfrastructureServices(config);

                // Session context
                services.AddSingleton<ICurrentUserContext, CurrentUserContext>();

                // Application Services
                services.AddScoped<IAuthorizationService, AuthorizationService>();
                services.AddScoped<AccountService>();
                services.AddScoped<UserService>();
                services.AddScoped<ItemService>();
                services.AddScoped<BorrowingService>();
                services.AddScoped<PolicyService>();

                services.AddSingleton<INavigationService, NavigationService>();

                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegisterViewModel>();
                services.AddSingleton<MainViewModel>();

                services.AddSingleton<MainView>();

                _rootProvider = services.BuildServiceProvider();

                _appScope = _rootProvider.CreateScope();
                var sp = _appScope.ServiceProvider;

                try
                {
                    // Throws ex if db init fails
                    var db = sp.GetRequiredService<LibraryDbContext>();

                    db.Database.EnsureCreated();
                    // db.Database.Migrate();

                    DbSeeder.SeedAsync(db).GetAwaiter().GetResult();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"DB init failed: {ex}");
                }

                var mainWindow = sp.GetRequiredService<MainView>();
                mainWindow.DataContext = sp.GetRequiredService<MainViewModel>();
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }

        private void DisableAvaloniaDataAnnotationValidation()
        {
            var dataValidationPluginsToRemove =
                BindingPlugins.DataValidators.OfType<DataAnnotationsValidationPlugin>().ToArray();

            foreach (var plugin in dataValidationPluginsToRemove)
            {
                BindingPlugins.DataValidators.Remove(plugin);
            }
        }
    }
}