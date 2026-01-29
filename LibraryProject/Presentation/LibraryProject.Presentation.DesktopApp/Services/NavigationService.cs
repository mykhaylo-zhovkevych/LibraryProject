using LibraryProject.Presentation.DesktopApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Services
{
    public class NavigationService : INavigationService
    {
        private MainViewModel _mainViewModel;
        private readonly IServiceProvider _serviceProvider;


        public NavigationService(IServiceProvider serviceProvider )
        {
            _serviceProvider = serviceProvider;

        }

        public Task NavigateTo<T>() where T : ViewModelBase
        {
            if (_mainViewModel == null)
            {
                return Task.CompletedTask;
            }

            var viewModel = _serviceProvider.GetRequiredService<T>();

            _mainViewModel.CurrentViewModel = viewModel;
            return Task.CompletedTask;

        }

        public void SetMainViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

    }
}
