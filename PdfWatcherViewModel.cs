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
            FolderStatus = new Status();
            Folder = AppDomain.CurrentDomain.BaseDirectory;

            Watcher = new FileSystemWatcher
            {
                Path = Folder,
                NotifyFilter = NotifyFilters.LastWrite,
                Filter = "*.pdf",
                EnableRaisingEvents = true
            };

            Watcher.Changed += Watcher_Changed;
        }

        public void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            FInfo = new FileInfo(e.FullPath);
        }

        private FileSystemWatcher _watcher;
        public FileSystemWatcher Watcher
        {
            get => _watcher;
            set => SetValue(ref _watcher, value);
        }

        private FileInfo _fInfo;
        public FileInfo FInfo
        {
            get => _fInfo;
            set { 
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
            set
            {
                if (SetValue(ref _folder, value))
                {
                    ChangeFolder();
                }
            }
        }

        //private string _filePath;
        //public string FilePath
        //{
        //    get => _filePath;
        //    set
        //    {
        //        if (SetValue(ref _filePath, value))
        //        {
        //            NotifyPropertyChanged(nameof(FileName));
        //            NotifyPropertyChanged(nameof(FileUri));
        //        }
        //    }
        //}

        private Status _folderStatus;
        public Status FolderStatus
        {
            get => _folderStatus;
            set => SetValue(ref _folderStatus, value);
        }

        //public string? FileName
        //{
        //    get
        //    {
        //        if (!string.IsNullOrEmpty(FilePath))
        //            return Path.GetFileName(FilePath);
        //        else
        //            return null;
        //    }
        //}

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

        private void ChangeFolder()
        {
            if (!string.IsNullOrEmpty(Folder) && Directory.Exists(Folder) && Watcher != null)
                Watcher.Path = Folder;
        }
    }

    public class Status : NotifyBase
    {
        public Status()
        {
            StatusColor = Brushes.Green;
            Valid = true;
        }

        public void SetStatus(bool valid)
        {
            Valid = valid;
            if (Valid)
            {
                StatusColor = Brushes.Black;
                Text = "";
            }
            else
            {
                StatusColor = Brushes.Red;
                Text = "Invalid Folder.";
            }
        }

        private Brush _statusColor;
        public Brush StatusColor
        {
            get => _statusColor;
            set => SetValue(ref _statusColor, value);
        }

        private string? _text;
        public string? Text
        {
            get => _text;
            set => SetValue(ref _text, value);
        }

        private bool _valid;
        public bool Valid
        {
            get => _valid;
            set => SetValue(ref _valid, value);
        }
    }
}
