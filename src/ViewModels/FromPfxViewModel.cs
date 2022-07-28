using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Input;
using PFXSplitter.Properties;
using SysadminsLV.WPF.OfficeTheme.Toolkit.Commands;

namespace PFXSplitter.ViewModels {
    class FromPfxViewModel : AsyncViewModel {
        String pfxFilePath,
               outDirPath,
               outFileNamePrefix;

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
                                         !String.IsNullOrEmpty(OutFileNamePrefix);
    }
}
