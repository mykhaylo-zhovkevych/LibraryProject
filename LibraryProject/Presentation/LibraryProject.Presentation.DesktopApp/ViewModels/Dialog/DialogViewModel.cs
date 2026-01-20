using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Presentation.DesktopApp.ViewModels.Dialog
{
    public partial class DialogViewModel : ViewModelBase
    {
        // Is a dialog open? 
        [ObservableProperty]
        private bool _isDialogOpen;

        protected TaskCompletionSource closeTask = new TaskCompletionSource();

        public async Task WaitDialogAsnyc()
        {
            await closeTask.Task;
        }

        public void Show()
        {
            if (closeTask.Task.IsCompleted)
                closeTask = new TaskCompletionSource();
            
            IsDialogOpen = true;
        }

        public void Close()
        {
            IsDialogOpen = false;

            closeTask.TrySetResult();
        }
    }
}
