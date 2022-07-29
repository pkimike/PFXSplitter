using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using PFXSplitter.Properties;
using SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

namespace PFXSplitter.ViewModels {
    class FromPfxViewModel : AsyncViewModel {
        const String PASSWORDS_DONT_MATCH = "Passwords must match";

        String pfxFilePath,
               outDirPath,
               outFileNamePrefix,
               password,
               confirmPassword;
               
        String misMatchedPasswords = String.Empty;

        Boolean usePassword,
                passwordsMatch;

        public FromPfxViewModel() {
            SelectPfxFileCommand = new RelayCommand(selectPfxFile);
            SetOutDirPathCommand = new RelayCommand(selectDir);
            SplitPfxCommand = new RelayCommand(splitPfx, canSplitPfx);
        }

        public ICommand SelectPfxFileCommand { get;}
        public ICommand SetOutDirPathCommand { get;}
        public ICommand SplitPfxCommand { get; }

        public String PfxFilePath {
            get => pfxFilePath;
            set {
                pfxFilePath = value;
                OnPropertyChanged(nameof(PfxFilePath));
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

        public String Password {
            get => password;
            set {
                password = value;
                PasswordsMatch = Password.Equals(ConfirmPassword, StringComparison.InvariantCulture);
                OnPropertyChanged(nameof(Password));
            }
        }

        public String ConfirmPassword {
            get => confirmPassword;
            set {
                confirmPassword = value;
                PasswordsMatch = Password.Equals(ConfirmPassword, StringComparison.InvariantCulture);
                OnPropertyChanged(nameof(ConfirmPassword));
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

        String getPemFile() => getFilePath("pem");
        String getKeyFile() => getFilePath("key");
        String getFilePath(String extension) {
            if (String.IsNullOrEmpty(OutDirPath) || String.IsNullOrEmpty(OutFileNamePrefix)) {
                return String.Empty;
            }

            return $"{OutDirPath}\\{OutFileNamePrefix}.{extension}";
        }
        void selectDir(Object o) {
            var dirBrowser = new FolderBrowserDialog {
                SelectedPath = Directory.GetCurrentDirectory(),
                ShowNewFolderButton = true
            };

            DialogResult result = dirBrowser.ShowDialog();
            if (result == DialogResult.OK) {
                OutDirPath = dirBrowser.SelectedPath;
            }
        }

        void selectPfxFile(Object o) {
            var fileBrowser = new OpenFileDialog {
                DefaultExt = ".pfx",
                Filter = Resources.Pkcs12FileExtensions,
                CheckFileExists = true,
                InitialDirectory = Environment.CurrentDirectory
            };

            DialogResult result = fileBrowser.ShowDialog();
            if (result == DialogResult.OK) {
                PfxFilePath = fileBrowser.FileName;
            }
        }

        void splitPfx(Object o) {

        }

        Boolean canSplitPfx(Object o) => !String.IsNullOrEmpty(OutDirPath) && 
                                         Directory.Exists(OutDirPath) && 
                                         !String.IsNullOrEmpty(PfxFilePath) && 
                                         File.Exists(PfxFilePath) && 
                                         !String.IsNullOrEmpty(OutFileNamePrefix) &&
                                         (PasswordsMatch || !UsePassword);
    }
}
