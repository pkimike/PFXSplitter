using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;

namespace PFXSplitter.ViewModels {
    class ToPfxViewModel : AsyncViewModel {
        readonly Action<String> _handler;

        String certFilePath,
               keyFilePath,
               pfxFolderPath,
               pfxFileName,
               password,
               confirmPassword;

        public ToPfxViewModel() {
            
        }

        public ICommand BrowseCertFileCommand { get; set; }
        public ICommand BrowseKeyFileCommand { get; set; }
        public ICommand BrowsePfxFolderCommand { get; set; }
        public ICommand ConvertToPfxCommand { get; set; }

        public String CertFilePath { get; set; }
        public String KeyFilePath { get; set; }
        public String PfxFolderPath { get; set; }
        public String PfxFileName { get; set; }

        void selectCertFile(Object o) {
            CertFilePath = selectFile("pem");
        }

        void selectKeyFile(Object o) {
            KeyFilePath = selectFile("key");
        }

        void selectPfxFolder(Object o) {
            PfxFolderPath = selectDir();
        }

        static String selectFile(String extension) {
            String filter = $"{extension} files (*.{extension})|*.{extension}|All files (*.*)|*.*";
            var fileBrowser = new OpenFileDialog {
                Filter = filter,
                CheckFileExists = true,
                InitialDirectory = Environment.CurrentDirectory
            };

            DialogResult result = fileBrowser.ShowDialog();
            if (result == DialogResult.OK) {
                return fileBrowser.FileName;
            }

            return String.Empty;
        }
        static String selectDir() {
            var dirBrowser = new FolderBrowserDialog {
                SelectedPath = Directory.GetCurrentDirectory(),
                ShowNewFolderButton = true
            };

            DialogResult result = dirBrowser.ShowDialog();
            if (result == DialogResult.OK) {
                return dirBrowser.SelectedPath;
            }

            return String.Empty;
        }
    }
}
