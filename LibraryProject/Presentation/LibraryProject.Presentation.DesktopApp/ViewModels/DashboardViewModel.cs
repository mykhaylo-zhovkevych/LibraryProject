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


        public DashboardViewModel(ICurrentUserContext currentUser)
        {
            _currentUser = currentUser;
        }


        [RelayCommand]
        public void Logout()
        {
            _currentUser.SignOut();
        }

    }
}
