using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Pkcs12Converter.API;
using SysadminsLV.WPF.OfficeTheme.Toolkit;
using SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

namespace Pkcs12Converter.ViewModels {
    class FromPfxViewModel : DefaultViewModel {

        static readonly List<String> Pkcs12Extensions = new() { "pfx", "p12" };

        String pfxFilePath = String.Empty;
        String pfxPassword = String.Empty;
        

        public FromPfxViewModel() {
            SelectPfxFileCommand = new RelayCommand(selectPfxFile);
            SplitPfxCommand = new RelayCommand(splitPfx, canSplitPfx);
        }

        public ICommand SelectPfxFileCommand { get;}
        public ICommand SplitPfxCommand { get; }

        public String PfxFilePath {
            get => pfxFilePath;
            set {
                pfxFilePath = value;
                OnPropertyChanged(nameof(PfxFilePath));
            }
        }

        public String PfxPassword {
            get => pfxPassword;
            set {
                pfxPassword = value;
                OnPropertyChanged(nameof(PfxPassword));
            }
        }

        void selectPfxFile(Object o) {
            if (FileSystemUtilities.SelectFile(Pkcs12Extensions, false, out String selectedPath)) {
                PfxFilePath = selectedPath;
            }
        }
        void splitPfx(Object o) {
            Boolean result = Pkcs12Convert.ToCertAndKey(PfxFilePath,
                (String.IsNullOrEmpty(PfxPassword) ? null : PfxPassword),
                getFilePath("pem"),
                getFilePath("key"),
                (String.IsNullOrEmpty(Password) ? null : Password),
                out String error);

            if (result) {
                MsgBox.Show("Success", "Successfully extracted PEM-encoded certificate and key", MessageBoxImage.Information);
            } else {
                MsgBox.Show("Error", error);
            }
        }
        Boolean canSplitPfx(Object o) => !String.IsNullOrEmpty(OutDirPath) && 
                                         Directory.Exists(OutDirPath) && 
                                         !String.IsNullOrEmpty(PfxFilePath) && 
                                         File.Exists(PfxFilePath) && 
                                         !String.IsNullOrEmpty(OutFileNamePrefix) &&
                                         (PasswordsMatch || !UsePassword);
    }
}
