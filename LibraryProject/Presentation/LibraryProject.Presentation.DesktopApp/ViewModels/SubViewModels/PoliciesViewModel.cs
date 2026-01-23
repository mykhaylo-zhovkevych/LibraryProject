using LibraryProject.Presentation.DesktopApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels
{
    public partial class PoliciesViewModel : PageViewModel
    {

        public PoliciesViewModel()
        {
            PageName = ApplicationPageNames.ManagementPolicies;
        }

        //TODO: List of policies as for navigation and delete, update is inside of the list itslef
        //with a forum for policy creation

    }
}
