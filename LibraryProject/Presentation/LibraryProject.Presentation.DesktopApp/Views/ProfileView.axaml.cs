using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LibraryProject.Presentation.DesktopApp.ViewModels;

namespace LibraryProject.Presentation.DesktopApp.Views;

public partial class ProfileView : UserControl
{
    public ProfileView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += OnAttached;
    }

    private async void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is ProfileViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }
}