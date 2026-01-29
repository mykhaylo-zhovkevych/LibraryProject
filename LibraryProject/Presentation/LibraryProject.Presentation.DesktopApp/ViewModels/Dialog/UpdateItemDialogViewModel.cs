using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Presentation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.Dialog
{
    public partial class UpdateItemDialogViewModel : DialogViewModel
    {

        [ObservableProperty] private string _title = "";
        [ObservableProperty] private string _message = "";
        [ObservableProperty] private string _confirmText = "";
        [ObservableProperty] private string _cancelText = "";

        public DisplayedItem? SourceItem { get; }

        [ObservableProperty] private string _itemTitle = "";
        [ObservableProperty] private string _author = "";
        [ObservableProperty] private string _yearText = "";
        [ObservableProperty] private string _description = "";
        [ObservableProperty] private string _copiesText = "";

        [ObservableProperty]
        private bool _confirmed;

        public UpdateItemDialogViewModel(DisplayedItem item)
        {
            SourceItem = item;

            ItemTitle = item.Title;
            Author = item.Author;
            YearText = item.Year.ToString();
            Description = item.Description;
            CopiesText = item.AvailableCopies.ToString();
        }

        public int? Year => int.TryParse(YearText, out int year) ? year : null;
        public int? Copies => int.TryParse(CopiesText, out int copies) ? copies : null;


        [RelayCommand]
        public void Confirm()
        {
            Confirmed = true;
            Close();
        }

        [RelayCommand]
        public void Cancel()
        {
            Confirmed = false;
            Close();
        }

        public async Task<bool> WaitConfirmationAsync()
        {
            if (!Confirmed)
                await WaitDialogAsnyc();
            return Confirmed;
        }

    }
}
