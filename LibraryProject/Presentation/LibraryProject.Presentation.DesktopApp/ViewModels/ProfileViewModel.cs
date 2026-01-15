using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.ViewModels;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class ProfileViewModel : PageViewModel
    {
        public ProfileViewModel()
        {
            PageName = ApplicationPageNames.Profile;
        }
    }
}
