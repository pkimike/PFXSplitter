using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pkcs12Converter {
    static class FileSystemUtilities {
        public static Boolean SelectFile(IEnumerable<String> preferredExtensions, Boolean allowOtherExtensions, out String filePath) {
            filePath = String.Empty;
            var extensionsList = preferredExtensions.ToList();
            var fileBrowser = new OpenFileDialog {
                DefaultExt = $".{extensionsList[0]}",
                Filter = getFilter(extensionsList, allowOtherExtensions),
                CheckFileExists = true,
                InitialDirectory = Environment.CurrentDirectory
            };

            DialogResult result = fileBrowser.ShowDialog();
            if (result == DialogResult.OK) {
                filePath = fileBrowser.FileName;
            }

            return !String.IsNullOrEmpty(filePath);
        }

        public static Boolean SelectFile(String extension, Boolean allowOtherExtensions, out String filePath) {
            return SelectFile(new List<String> { extension }, allowOtherExtensions, out filePath);
        }
        public static Boolean SelectFolder(out String dirPath) {
            dirPath = String.Empty;
            var dirBrowser = new FolderBrowserDialog {
                SelectedPath = Directory.GetCurrentDirectory(),
                ShowNewFolderButton = true
            };

            DialogResult result = dirBrowser.ShowDialog();
            if (result == DialogResult.OK) {
                dirPath = dirBrowser.SelectedPath;
            }

            return !String.IsNullOrEmpty(dirPath);
        }

        public static Boolean FileExists(this String filePath) {
            if (String.IsNullOrWhiteSpace(filePath)) {
                return false;
            }

            return File.Exists(filePath);
        }
        public static Boolean DirectoryExists(this String directoryPath) {
            if (String.IsNullOrWhiteSpace(directoryPath)) {
                return false;
            }

            return Directory.Exists(directoryPath);
        }

        static String getFilter(List<String> preferredExtensions, Boolean allowOtherExtensions) {
            const String ALL_FILES = "All files(*.*) | *.*";

            if (!preferredExtensions.Any()) {
                return ALL_FILES;
            }
            StringBuilder sb = new StringBuilder();
            Boolean firstAdded = false;
            foreach (String extension in preferredExtensions) {
                if (firstAdded) {
                    sb.Append("|");
                } else {
                    firstAdded = true;
                }
                sb.Append($"{extension} files (*.{extension})|*.{extension}");
            }

            if (allowOtherExtensions) {
                sb.Append($"|{ALL_FILES}");
            }

            return sb.ToString();
        }
    }
}
