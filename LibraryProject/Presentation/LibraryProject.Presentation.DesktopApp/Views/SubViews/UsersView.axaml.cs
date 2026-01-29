using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels;

namespace LibraryProject.Presentation.DesktopApp.Views.SubViews;

public partial class UsersView : UserControl
{
    public UsersView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += OnAttached;
    }


    private async void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is UsersViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }
}