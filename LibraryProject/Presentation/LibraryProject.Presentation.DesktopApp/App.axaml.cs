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
using LibraryProject.Presentation.DesktopApp.Factories;
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

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override async void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                BindingPlugins.DataValidators.RemoveAt(0);
                DisableAvaloniaDataAnnotationValidation();

                // Configuration
                var config = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                // DI
                var services = new ServiceCollection().AddApplicationServices(config);

                var serviceProvider = services.BuildServiceProvider();

                // DB init in isolated scope
                using (var scope = serviceProvider.CreateScope())
                {

                    ItemService service = scope.ServiceProvider.GetRequiredService<ItemService>();
                    var db = scope.ServiceProvider.GetRequiredService<LibraryDbContext>();
                    await db.Database.EnsureCreatedAsync();
                    await DbSeeder.SeedAsync(db, service);
                }

                // UI (root scope)
                var mainViewModel = serviceProvider.GetRequiredService<MainViewModel>();
                var navigation = serviceProvider.GetRequiredService<INavigationService>();

                // Here hidden dependency on navigation service
                navigation.SetMainViewModel(mainViewModel);

                desktop.MainWindow = new MainView
                {
                    DataContext = mainViewModel
                };
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