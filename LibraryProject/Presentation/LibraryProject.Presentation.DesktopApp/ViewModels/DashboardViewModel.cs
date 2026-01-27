using Avalonia.Dialogs;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Interfaces;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Factories;
using LibraryProject.Presentation.DesktopApp.Services;
using LibraryProject.Presentation.DesktopApp.ViewModels;
using LibraryProject.Presentation.DesktopApp.ViewModels.Dialog;
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
        // DI
        private PageFactory _pageFactory;
        private readonly INavigationService _navigation;
        private readonly ICurrentUserContext _currentUser;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CatalogPageIsActive))]
        [NotifyPropertyChangedFor(nameof(BorrowingPageIsActive))]
        [NotifyPropertyChangedFor(nameof(ProfilePageIsActive))]
        [NotifyPropertyChangedFor(nameof(ManagementPageIsActive))]
        private PageViewModel _currentPage;

        public DashboardViewModel(PageFactory pageFactory, INavigationService navigation, ICurrentUserContext currentUser)
        {
            _pageFactory = pageFactory;
            _navigation = navigation;
            _currentUser = currentUser;
            GoToCatalog();
        }

        public bool CatalogPageIsActive => CurrentPage.PageName == ApplicationPageNames.Catalog;
        public bool BorrowingPageIsActive => CurrentPage.PageName == ApplicationPageNames.Borrowing;
        public bool ProfilePageIsActive => CurrentPage.PageName == ApplicationPageNames.Profile;
        public bool ManagementPageIsActive => CurrentPage.PageName == ApplicationPageNames.Management;


        [RelayCommand]
        public void GoToCatalog()
        { 
            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.Catalog);
        }

        [RelayCommand]
        private void GoToBorrowing()
        {
            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.Borrowing);
        }

        [RelayCommand]
        private void GoToProfile()
        {
            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.Profile);
        }

        [RelayCommand]
        private void GoToManagement()
        {
            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.Management);
        }

        [RelayCommand]
        public void Logout()
        {
            _currentUser.SignOut();
            _navigation.NavigateTo<LoginViewModel>();
        }
    }
}
