using LibraryProject.Presentation.DesktopApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels
{
    public partial class UsersViewModel : PageViewModel
    {

        public UsersViewModel()
        {
            PageName = ApplicationPageNames.ManagementUsers;
        }

        //TODO: list of the users and list of the account 
        // select user to see the rent hostory as apopup

    }
}
