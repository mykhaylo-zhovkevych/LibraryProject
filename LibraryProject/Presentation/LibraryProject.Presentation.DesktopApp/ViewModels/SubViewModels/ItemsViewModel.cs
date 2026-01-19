using LibraryProject.Presentation.DesktopApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels
{
    public partial class ItemsViewModel : PageViewModel
    {
        public ItemsViewModel()
        {
            PageName = ApplicationPageNames.ManagementItems;
        }
    }
}
