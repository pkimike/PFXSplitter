using System;
using System.Windows;
using System.Windows.Input;
using Pkcs12Converter.API;
using SysadminsLV.WPF.OfficeTheme.Toolkit;
using SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

namespace Pkcs12Converter.ViewModels {
    class ToPfxViewModel : DefaultViewModel {
        String certFilePath = String.Empty;
        String keyFilePath = String.Empty;

        public ToPfxViewModel() {
            SelectCertFileCommand = new RelayCommand(selectCertFile);
            SelectKeyFileCommand = new RelayCommand(selectKeyFile);
            ConvertToPfxCommand = new RelayCommand(convertToPfx, canConvert);
        }

        public ICommand SelectCertFileCommand { get; set; }
        public ICommand SelectKeyFileCommand { get; set; }
        public ICommand ConvertToPfxCommand { get; set; }

        public String CertFilePath {
            get => certFilePath;
            set {
                certFilePath = value;
                OnPropertyChanged(nameof(CertFilePath));
            }
        }

        public String KeyFilePath {
            get => keyFilePath;
            set {
                keyFilePath = value;
                OnPropertyChanged(nameof(KeyFilePath));
            }
        }

        void selectCertFile(Object o) {
            if (FileSystemUtilities.SelectFile("pem", true, out String selectedFile)) {
                CertFilePath = selectedFile;
            }
        }

        void selectKeyFile(Object o) {
            if (FileSystemUtilities.SelectFile("key", true, out String selectedFile)) {
                KeyFilePath = selectedFile;
            }
        }
        void convertToPfx(Object o) {
            Boolean result = Pkcs12Convert.ToPkcs12(CertFilePath, KeyFilePath,
                (String.IsNullOrEmpty(Password) ? null : Password), getFilePath("pfx"), out String error);
            if (result) {
                MsgBox.Show("Success", "Successfully extracted PEM-encoded certificate and key", MessageBoxImage.Information);
            } else {
                MsgBox.Show("Error", error);
            }
        }

        Boolean canConvert(Object o) {
            return CertFilePath.FileExists() &&
                   KeyFilePath.FileExists() &&
                   OutDirPath.DirectoryExists() &&
                   !String.IsNullOrEmpty(OutFileNamePrefix) &&
                   (!UsePassword || PasswordsMatch);
        }
    }
}
