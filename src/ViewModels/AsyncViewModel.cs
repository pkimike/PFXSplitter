using System;
using SysadminsLV.WPF.OfficeTheme.Toolkit.ViewModels;

namespace PFXSplitter.ViewModels {
    public class AsyncViewModel : ViewModelBase {
        Boolean isBusy;

        public Boolean IsBusy {
            get => isBusy;
            set {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        protected virtual void OnConfigurationChanged() {
            EventHandler handler = ConfigurationChanged;
            handler?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ConfigurationChanged;
    }
}
