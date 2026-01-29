using Avalonia;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Domain.Enum;
using LibraryProject.Presentation.DesktopApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.Dialog
{
    public partial class UpdatePolicyDialogViewModel : DialogViewModel
    {
        [ObservableProperty] private string _title = "";
        [ObservableProperty] private string _message = "";
        [ObservableProperty] private string _confirmText = "";
        [ObservableProperty] private string _cancelText = "";

        public DisplayedPolicy? SourcePolicy { get; }

        [ObservableProperty] private string _policyTitle = "";
        [ObservableProperty] private string _newPolicyUserType;
        [ObservableProperty] private string _newPolicyItemType;


        [ObservableProperty] private int _newPolicyExtensions = 0;
        [ObservableProperty] private int _newPolicyLoadPeriodInDays = 0;
        [ObservableProperty] private decimal _newPolicyFees = 0;

        //public ObservableCollection<UserType> UserTypes { get; } = new ObservableCollection<UserType>(Enum.GetValues<UserType>());
        //public ObservableCollection<ItemType> ItemTypes { get; } = new ObservableCollection<ItemType>(Enum.GetValues<ItemType>());


        [ObservableProperty]
        private bool _confirmed;

        public UpdatePolicyDialogViewModel(DisplayedPolicy policy)
        {
            SourcePolicy = policy;

            NewPolicyItemType = policy.PolicyItemType;
            NewPolicyUserType = policy.PolicyUserType;

            PolicyTitle = policy.PolicyName;
            NewPolicyExtensions = policy.PolicyExtensions;
            NewPolicyLoadPeriodInDays = policy.PolicyLoanPeriodInDays;
            NewPolicyFees = policy.PolicyLoanFees;
        }


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
