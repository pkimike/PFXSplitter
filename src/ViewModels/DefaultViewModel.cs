using System;
using System.Windows.Input;
using SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;
using SysadminsLV.WPF.OfficeTheme.Toolkit.ViewModels;

namespace Pkcs12Converter.ViewModels {
    public class DefaultViewModel : ViewModelBase {
        const String PASSWORDS_DONT_MATCH = "Passwords must match";

        Boolean isBusy;

        String password = String.Empty;
        String confirmPassword = String.Empty;
        String misMatchedPasswords = String.Empty;
        String outDirPath = String.Empty; 
        String outFileNamePrefix = String.Empty;

        Boolean usePassword,
                passwordsMatch;

        public DefaultViewModel() {
            SetOutDirPathCommand = new RelayCommand(selectDir);
        }

        public ICommand SetOutDirPathCommand { get; }


        public Boolean IsBusy {
            get => isBusy;
            set {
                isBusy = value;
                OnPropertyChanged(nameof(IsBusy));
            }
        }

        public String Password {
            get => password;
            set {
                password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        public String ConfirmPassword {
            get => confirmPassword;
            set {
                confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }
        public Boolean UsePassword {
            get => usePassword;
            set {
                usePassword = value;
                if (!usePassword) {
                    Password = String.Empty;
                    ConfirmPassword = String.Empty;
                    MisMatchedPasswords = String.Empty;
                }
                OnPropertyChanged(nameof(UsePassword));
            }
        }
        public String MisMatchedPasswords {
            get => misMatchedPasswords;
            set {
                misMatchedPasswords = value;
                OnPropertyChanged(nameof(MisMatchedPasswords));
            }
        }
        public Boolean PasswordsMatch {
            get => passwordsMatch;
            set {
                passwordsMatch = value;
                if (String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(ConfirmPassword) || passwordsMatch) {
                    MisMatchedPasswords = String.Empty;
                } else {
                    MisMatchedPasswords = PASSWORDS_DONT_MATCH;
                }
            }
        }
        public String OutDirPath {
            get => outDirPath;
            set {
                outDirPath = value;
                OnPropertyChanged(nameof(OutDirPath));
            }
        }
        public String OutFileNamePrefix {
            get => outFileNamePrefix;
            set {
                outFileNamePrefix = value;
                OnPropertyChanged(nameof(OutFileNamePrefix));
            }
        }

        void selectDir(Object o) {
            if (FileSystemUtilities.SelectFolder(out String selectedPath)) {
                OutDirPath = selectedPath;
            }
        }
        protected String getFilePath(String extension) {
            if (String.IsNullOrEmpty(OutDirPath) || String.IsNullOrEmpty(OutFileNamePrefix)) {
                return String.Empty;
            }

            return $"{OutDirPath}\\{OutFileNamePrefix}.{extension}";
        }
    }
}
