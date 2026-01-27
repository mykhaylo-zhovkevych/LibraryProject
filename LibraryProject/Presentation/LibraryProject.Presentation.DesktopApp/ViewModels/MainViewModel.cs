using CommunityToolkit.Mvvm.ComponentModel;
using LibraryProject.Presentation.DesktopApp.Services;

namespace LibraryProject.Presentation.DesktopApp.ViewModels
{
    public partial class MainViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        [ObservableProperty] private ViewModelBase _currentViewModel;
    
        public MainViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            ((NavigationService)navigationService).SetMainViewModel(this);
            navigationService.NavigateTo<LoginViewModel>();
        }

    }
}
