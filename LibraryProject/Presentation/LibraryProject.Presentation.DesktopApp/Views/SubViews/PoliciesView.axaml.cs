using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels;

namespace LibraryProject.Presentation.DesktopApp.Views.SubViews;

public partial class PoliciesView : UserControl
{
    public PoliciesView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += OnAttached;
    }


    private async void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is PoliciesViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }
}