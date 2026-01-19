using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.Linq;

namespace LibraryProject.Presentation.DesktopApp.Views.SubViews;

public partial class ItemsView : UserControl
{
    public ItemsView()
    {
        InitializeComponent();
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
}