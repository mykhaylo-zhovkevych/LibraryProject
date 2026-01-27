using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class ManagementViewModel : PageViewModel
    {
        private PageFactory _pageFactory;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ItemsViewIsActive))]
        [NotifyPropertyChangedFor(nameof(UsersViewIsActive))]
        [NotifyPropertyChangedFor(nameof(PoliciesViewIsActive))]
        private PageViewModel _currentPage;

        public ManagementViewModel(PageFactory pageFactory)
        {
            _pageFactory = pageFactory;

            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.ManagementItems);
        }


        public bool ItemsViewIsActive => CurrentPage.PageName == ApplicationPageNames.ManagementItems;
        public bool UsersViewIsActive => CurrentPage.PageName == ApplicationPageNames.ManagementUsers;
        public bool PoliciesViewIsActive => CurrentPage.PageName == ApplicationPageNames.ManagementPolicies;


        [RelayCommand]
        public void GoToItemsView()
        {
            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.ManagementItems);
        }

        [RelayCommand]
        public void GoToUsersView()
        {
            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.ManagementUsers);
        }

        [RelayCommand]
        public void GoToPoliciesView()
        {
            CurrentPage = _pageFactory.GetPageViewModel(ApplicationPageNames.ManagementPolicies);
        }
    }
}
