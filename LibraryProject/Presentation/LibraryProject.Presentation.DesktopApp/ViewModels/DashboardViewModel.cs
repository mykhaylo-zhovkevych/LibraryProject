using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Presentation.DesktopApp.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class DashboardViewModel : ViewModelBase
    {
        private readonly ICurrentUserContext _currentUser;
        private readonly IServiceProvider _serviceProvider;

        // ---
        [ObservableProperty] private ViewModelBase _currentPage;

        //private readonly ProfileViewModel _profilePage = new();
        //private readonly CatalogViewModel _catalogPage = new();
        //private readonly BorrowingViewModel _borrowingPage = new();
        //private readonly ManagementViewModel _managementPage = new();
        // ---

        // Does this code needs a factory pattern?
        public DashboardViewModel(ICurrentUserContext currentUser, IServiceProvider serviceProvider)
        {
            // _homePage = homePage
            _currentUser = currentUser;
            _serviceProvider = serviceProvider;
            CurrentPage = _serviceProvider.GetRequiredService<CatalogViewModel>();
        }


        [RelayCommand]
        public void GoToCatalog()
        { 
            CurrentPage = _serviceProvider.GetRequiredService<CatalogViewModel>();
        }

        [RelayCommand]
        private void GoToBorrowing()
        {
            CurrentPage = _serviceProvider.GetRequiredService<BorrowingViewModel>();
        }


        [RelayCommand]
        private void GoToProfile()
        {
            CurrentPage = _serviceProvider.GetRequiredService<ProfileViewModel>();
        }

        [RelayCommand]
        private void GoToManagement()
        {
            CurrentPage = _serviceProvider.GetRequiredService<ManagementViewModel>();
        }

        [RelayCommand]
        public void Logout()
        {
            _currentUser.SignOut();
        }

    }
}
