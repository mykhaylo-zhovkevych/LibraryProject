using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.Views.Controlls
{
    public partial class CopyTextButton : UserControl
    {
        public static readonly StyledProperty<string?> TextProperty =
             AvaloniaProperty.Register<CopyTextButton, string?>(nameof(Text));

        public string? Text
        {
            get => GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        private TopLevel? _topLevel;

        public CopyTextButton()
        {

            InitializeComponent();

            this.AttachedToVisualTree += OnAttachedToVisualTree;
            this.DetachedFromVisualTree += OnDetachedFromVisualTree;


            Button? btn = this.FindControl<Button>("PART_Button");
            if (btn == null) return;

            // Initial display
            btn.Content = Text;

            btn.Click += async (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(Text))
                {
                    return;
                }

                TopLevel? top = _topLevel ?? TopLevel.GetTopLevel(this);

                try
                {
                    await top.Clipboard.SetTextAsync(Text);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex);
                }

                // Simple feedback
                Object? original = btn.Content;
                btn.Content = "Kopiert!";
            };

        }


        private void InitializeComponent() => AvaloniaXamlLoader.Load(this);

        private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            _topLevel = TopLevel.GetTopLevel(this);
        }

        private void OnDetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
        {
            _topLevel = null;
        }
    }
}