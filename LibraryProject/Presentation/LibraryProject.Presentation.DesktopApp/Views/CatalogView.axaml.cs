using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LibraryProject.Presentation.DesktopApp.ViewModels;

namespace LibraryProject.Presentation.DesktopApp.Views;

public partial class CatalogView : UserControl
{
    public CatalogView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += OnAttached;
    }

    private async void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is CatalogViewModel vm)
        {
            await vm.InitializedAsync();
        }
    }
}