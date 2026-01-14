using CommunityToolkit.Mvvm.ComponentModel;
using LibraryProject.Presentation.DesktopApp.Services;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    // Partial in this context allwos OberservableProperty change this class
    public partial class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        [ObservableProperty] private ViewModelBase _currentViewModel;
    
        // 338c427c-fcc4-4ea0-91e8-62a86594fded

        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ((NavigationService)navigationService).SetMainViewModel(this);
            navigationService.NavigateTo<LoginViewModel>();
        }

    }
}
