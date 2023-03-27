using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Brushes = System.Windows.Media.Brushes;

namespace PdfWatcher
{
    public class PdfWatcherViewModel : NotifyBase
    {
        public PdfWatcherViewModel()
        {
            var folderSettings = UserSettings.Default.Folder;
            if (!string.IsNullOrEmpty(folderSettings))
                Folder = folderSettings;
            else
                Folder = AppDomain.CurrentDomain.BaseDirectory;
        }

        private FileInfo _fInfo;
        public FileInfo FInfo
        {
            get => _fInfo;
            set
            {
                if (SetValue(ref _fInfo, value))
                {
                    NotifyPropertyChanged(nameof(FileUri));
                }
            }
        }

        private string _folder;
        public string Folder
        {
            get => _folder;
            set => SetValue(ref _folder, value);
        }

        public string WatcherFolder { get => GetDirectory(Folder); }

        public Uri FileUri
        {
            get
            {
                if (FInfo != null)
                    return new Uri(FInfo.FullName);
                else
                    return new Uri("about:blank");
            }
        }

        private string GetDirectory(string folder)
        {
            if (!folder.EndsWith("\\"))
                return folder + "\\";
            else
                return folder;
        }

        private bool _showNoPdfText;
        public bool ShowNoPdfText
        {
            get => _showNoPdfText;
            set => SetValue(ref _showNoPdfText, value);
        }
    }
}
