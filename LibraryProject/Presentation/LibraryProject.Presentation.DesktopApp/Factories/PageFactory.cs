using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Factories
{
    public class PageFactory
    {
        private readonly Func<ApplicationPageNames, PageViewModel> _pageFactory;

        public PageFactory(Func<ApplicationPageNames, PageViewModel> factory) 
        {
            _pageFactory = factory;
        }

        // The switch gets stored and get run only when the mehtod called in here, like a pointer to function
        public PageViewModel GetPageViewModel(ApplicationPageNames pageName) => _pageFactory.Invoke(pageName);
        

    }               
}