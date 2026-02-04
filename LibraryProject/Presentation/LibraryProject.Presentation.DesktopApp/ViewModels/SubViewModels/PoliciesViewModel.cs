using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LibraryProject.Application.Services;
using LibraryProject.Domain.Entities;
using LibraryProject.Domain.Enum;
using LibraryProject.Infrastructure.Persistence.InSqlite.Entities;
using LibraryProject.Presentation.DesktopApp.Data;
using LibraryProject.Presentation.DesktopApp.Models;
using LibraryProject.Presentation.DesktopApp.ViewModels.Dialog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.SubViewModels
{
    public partial class PoliciesViewModel : PageViewModel
    {
        private readonly PolicyService _policyService;
        private bool _initialized;

        [ObservableProperty] private string _newPolicyName = "";
        [ObservableProperty] private string _newPolicyExtensionsText = "1";
        [ObservableProperty] private string _newPolicyLoadPeriodInDaysText = "7";
        [ObservableProperty] private string _newPolicyFeesText = "0.05";


        [ObservableProperty]
        private DisplayedPolicy? _selectedPolicy;

        [ObservableProperty]
        private UserType _selectedUserType = UserType.Admin;

        [ObservableProperty]
        private ItemType _selectedItemType = ItemType.Book;


        public ObservableCollection<DisplayedPolicy> Policies { get; } = new ObservableCollection<DisplayedPolicy>();

        public ObservableCollection<UserType> UserTypes { get; } = new ObservableCollection<UserType>(Enum.GetValues<UserType>());
        public ObservableCollection<ItemType> ItemTypes { get; } = new ObservableCollection<ItemType>(Enum.GetValues<ItemType>());

        public uint NewPolicyExtensions => uint.TryParse(NewPolicyExtensionsText, out var e) ? e : 0; 
        public uint NewPolicyLoadPeriodInDays => uint.TryParse(NewPolicyLoadPeriodInDaysText, out var lp) ? lp : 0;
        public decimal NewPolicyFees => decimal.TryParse(NewPolicyFeesText, out var e) ? e : 0;


        public PoliciesViewModel(PolicyService policyService)
        {
            _policyService = policyService;

            PageName = ApplicationPageNames.ManagementPolicies;
        }


        public async Task InitializeAsync()
        {
            if (_initialized) return;
            _initialized = true;

            await LoadPoliciesAsync();
        }


        [RelayCommand]
        private void CancelCreate() => ResetCreateForm();

        private void ResetCreateForm()
        {
            NewPolicyName = "";
            NewPolicyExtensionsText = "0";
            NewPolicyLoadPeriodInDaysText = "0";
            NewPolicyFeesText = "0,00";
            SelectedUserType = UserType.Admin;
            SelectedItemType = ItemType.Book;
        }

        [RelayCommand]
        private async Task CreatePolicyAsync(CancellationToken ct = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewPolicyName))
                {
                    throw new ArgumentException("Richtlinie Name ist leer.");
                }

                if (string.IsNullOrWhiteSpace(NewPolicyExtensionsText))
                {
                    throw new ArgumentException("Erweiterungen ist leer.");
                }

                if (string.IsNullOrWhiteSpace(NewPolicyLoadPeriodInDaysText))
                {
                    throw new ArgumentException("Leihfrist ist leer.");
                }

                if (string.IsNullOrWhiteSpace(NewPolicyFeesText))
                {
                    throw new ArgumentException("Leihsteuern ist ungültig.");
                }

                Policy policy = new Policy(NewPolicyName, NewPolicyExtensions, NewPolicyFees, NewPolicyLoadPeriodInDays);

                await _policyService.AddPolicyAsync(SelectedUserType, SelectedItemType, policy, ct);

                ResetCreateForm();
                await LoadPoliciesAsync();

            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        [RelayCommand]
        private async Task ShowDeletePolicyDialogAsync()
        {
            if (SelectedPolicy == null)
            {
                ShowError("Keine Richtlinie ausgewählt.");
                return;
            }

            ConfirmDialogViewModel dialog = new ConfirmDialogViewModel()
            {
                Title = "Richtlinie löschen",
                Message = $"Möchten Sie die Richtlinie {SelectedPolicy.PolicyName} löschen?",
                ConfirmText = "Löschne",
                CancelText = "Abrechnen"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    await _policyService.RemovePolicyAsync(SelectedUserType, SelectedItemType, SelectedPolicy.PolicyName, default);

                    await LoadPoliciesAsync();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }


        [RelayCommand]
        private async Task ShowUpdatePolicyDialogAsync()
        {
            if (SelectedPolicy == null)
            {
                ShowError("Keine Richtlinie ausgewählt.");
                return;
            }

            UpdatePolicyDialogViewModel dialog = new UpdatePolicyDialogViewModel(SelectedPolicy)
            {
                Title = "Richtlinie aktualisieren",
                Message = $"Möchten Sie die Richtlinie {SelectedPolicy.PolicyName} aktualisieren?",
                ConfirmText = "Aktualisieren",
                CancelText = "Abbrechen"
            };

            CurrentDialog = dialog;
            dialog.Show();

            if (await dialog.WaitConfirmationAsync())
            {
                try
                {
                    string userTypeText = dialog.NewPolicyUserType ?? dialog.SourcePolicy?.PolicyUserType ?? "";
                    string itemTypeText = dialog.NewPolicyItemType ?? dialog.SourcePolicy?.PolicyItemType ?? "";

                    if (!Enum.TryParse<UserType>(userTypeText, out var userType)) throw new ArgumentException("Ungültiger Benutzertyp.");

                    if (!Enum.TryParse<ItemType>(itemTypeText, out var itemType)) throw new ArgumentException("Ungültiger Elementtyp.");

                    uint extensions = (uint)dialog.NewPolicyExtensions;
                    uint loanPeriod = (uint)dialog.NewPolicyLoadPeriodInDays;
                    decimal fees = dialog.NewPolicyFees;

                    await _policyService.UpdatePolicyValuesAsync(
                        userType,
                        itemType,
                        extensions,
                        fees,
                        loanPeriod,
                        default
                    );

                    await LoadPoliciesAsync();
                }
                catch (Exception ex)
                {
                    ShowError(ex.Message);
                }
            }
        }

        private async Task LoadPoliciesAsync(CancellationToken ct = default)
        {
            List<(UserType, ItemType, Policy)> policies = await _policyService.ReceiveExistingPoliciesWithTypesAsync(ct);

            Policies.Clear();
            foreach ((UserType, ItemType, Policy) p in policies)
            {
                ct.ThrowIfCancellationRequested();
                Policies.Add(MapPolicyToDisplayedPolicy(p));
            }
        }

        private static DisplayedPolicy MapPolicyToDisplayedPolicy((UserType, ItemType, Policy) policy)
        {
            return new DisplayedPolicy(
                policy.Item3.PolicyName,
                (int)policy.Item3.Extensions,
                policy.Item3.LoanFees,
                (int)policy.Item3.LoanPeriodInDays,
                policy.Item2.ToString(),
                policy.Item1.ToString()
            );
        }

        private void ShowError(string message)
        {
            CurrentDialog = new ErrorDialogViewModel
            {
                Title = "Fehler",
                Message = message,
                ConfirmText = "OK"
            };
            CurrentDialog.Show();
        }
    }
}