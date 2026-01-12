using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using LibraryProject.Presentation.DesktopApp.Services;
using LibraryProject.Presentation.DesktopApp.ViewModels;
using LibraryProject.Presentation.DesktopApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace LibraryProject.Presentation.DesktopApp
{
    public partial class App : Application
    {
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

                var services = new ServiceCollection();

                services.AddSingleton<INavigationService, NavigationService>();

                services.AddTransient<LoginViewModel>();
                services.AddTransient<RegisterViewModel>();
                services.AddSingleton<MainViewModel>();

                // What the point og having mainview
                services.AddSingleton<MainView>();

                var serviceProvider = services.BuildServiceProvider();

                var mainWindow = serviceProvider.GetRequiredService<MainView>();
                mainWindow.DataContext = serviceProvider.GetRequiredService<MainViewModel>();

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