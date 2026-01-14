using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
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


        // ---
        [ObservableProperty] private ViewModelBase _currentPage;

        private readonly ProfileViewModel _profilePage = new();
        private readonly CatalogViewModel _catalogPage = new();
        private readonly BorrowingViewModel _borrowingPage = new();
        private readonly ManagementViewModel _managementPage = new();
        // ---


        public DashboardViewModel(ICurrentUserContext currentUser)
        {
            _currentUser = currentUser;
            CurrentPage = _catalogPage;
        }


        [RelayCommand]
        public void GoToCatalog()
        { 
            CurrentPage = _catalogPage;
        }

        [RelayCommand]
        private void GoToBorrowing()
        {
            CurrentPage = _borrowingPage;
        }


        [RelayCommand]
        private void GoToProfile()
        {
            CurrentPage = _profilePage;
        }

        [RelayCommand]
        private void GoToManagement()
        {
            CurrentPage = _managementPage;
        }

        [RelayCommand]
        public void Logout()
        {
            _currentUser.SignOut();
        }

    }
}
