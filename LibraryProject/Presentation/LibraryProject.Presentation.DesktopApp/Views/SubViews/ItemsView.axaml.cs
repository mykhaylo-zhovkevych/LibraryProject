using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Views.SubViews;

public partial class ItemsView : UserControl
{
    public ItemsView()
    {
        InitializeComponent();
        this.AttachedToVisualTree += OnAttached;
    }

    private void NumericTextBox_OnTextInput(object? sender, Avalonia.Input.TextInputEventArgs e)
    {
        if (string.IsNullOrEmpty(e.Text)) return;

        if (e.Text.Any(ch => !char.IsDigit(ch)))
        {
            // It stops from processing
            e.Handled = true;        
        }

    }
    private async void OnAttached(object? sender, VisualTreeAttachmentEventArgs e)
    {
        if (DataContext is ItemsViewModel vm)
        {
            await vm.InitializeAsync();
        }
    }
}